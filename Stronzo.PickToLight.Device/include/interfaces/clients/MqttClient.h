#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>
#include <ArduinoNvs.h>
#include <PubSubClient.h>
#include <WiFiClientSecure.h>

#include "commands/DeviceCommand.h"
#include "utils/ProjUtils.h"

typedef std::function<void()> ConnectionCallback;
typedef std::function<void(DeviceCommand)> ExecuteCommandCallback;
typedef std::function<void(char*, uint8_t*, unsigned int)> MqttMessageReceivedCallback;

const ExecuteCommandCallback DEFAULT_CALLBACK = [](DeviceCommand commmand) {};

/* TOPIC STRUCTURE
**  {deviceId}/ping -> device pings every few seconds to keep-alive
**  {deviceId}/status -> device sends its info here when requested
**  {deviceId}/commands -> device receives commands here
*/

class MqttClient {
 private:
  uint32_t _pingTimeout = 0;
  bool _connected = false;
  char _receiveBuffer[MAX_MQTT_RECEIVE_BUFFER_SIZE] = "";

  std::string _deviceId = "";
  std::string _deviceToken = "";

  WiFiClientSecure _mqttWifiClient;
  PubSubClient _mqttClient;

  ConnectionCallback _onConnect = []() {};
  ConnectionCallback _onDisconnect = []() {};
  ExecuteCommandCallback _onLightsOnCommandReceived = DEFAULT_CALLBACK;
  ExecuteCommandCallback _onLightsOffCommandReceived = DEFAULT_CALLBACK;

  ExecuteCommandCallback _selectCallbackByCommandType(CommandType type) {
    switch (type) {
      case LightOn:
        return _onLightsOnCommandReceived;
      case LightOff:
        return _onLightsOffCommandReceived;
      default:
        return [](DeviceCommand command) {};
    }
  }

  void _connectAndSubscribe() {
    if (!_mqttClient.connect(_deviceId.c_str(), CONFIG_MQTT_USERNAME, CONFIG_MQTT_PASSWORD)) {
      log_e("Connection to MQTT server has failed.");
    } else {
      if (!_mqttClient.subscribe((_deviceId + "/commands").c_str(), 1)) {
        log_e("Unable to subscribe to commands topic...");
      }
    }
  }

  void _handleCommandReceived(DeviceCommand& command) {
    auto selectedCallback = _selectCallbackByCommandType(command.type);
    selectedCallback(command);
  }

  MqttMessageReceivedCallback _handleMqttReceive =
      [this](char* topicName, uint8_t* payload, unsigned int payloadLength) {
        strncpy(
            _receiveBuffer,
            (char*)payload,
            payloadLength > MAX_MQTT_RECEIVE_BUFFER_SIZE - 1 ? MAX_MQTT_RECEIVE_BUFFER_SIZE - 1 : payloadLength);

        log_d("Processing message on topic: %s -> %s : Length: %d", topicName, this->_receiveBuffer, payloadLength);

        StaticJsonDocument<MAX_MQTT_RECEIVE_BUFFER_SIZE> jsonDoc;
        DeserializationError err = deserializeJson(jsonDoc, _receiveBuffer, MAX_MQTT_RECEIVE_BUFFER_SIZE - 1);

        if (!err) {
          DeviceCommand receivedDeviceCommand = jsonDoc.as<DeviceCommand>();
          _handleCommandReceived(receivedDeviceCommand);
        } else {
          log_w("Received invalid command %s", err.c_str());
        }
      };

 public:
  MqttClient() {
    _mqttWifiClient.setInsecure();
    _mqttClient.setCallback(_handleMqttReceive);
  }

  MqttClient(const char* mqttEndpoint, const std::string& deviceId, const std::string& deviceToken)
      : _mqttClient(PubSubClient(mqttEndpoint, 8883, _mqttWifiClient)), _deviceId(deviceId), _deviceToken(deviceToken) {
    _mqttWifiClient.setInsecure();
    _mqttClient.setCallback(_handleMqttReceive);
  }

  void setOnConnectCallback(ConnectionCallback callback) { _onConnect = callback; }
  void setOnDisconnectCallback(ConnectionCallback callback) { _onDisconnect = callback; }
  void setOnLightOnCallback(ExecuteCommandCallback callback) { _onLightsOnCommandReceived = callback; }
  void setOnLightOffCallback(ExecuteCommandCallback callback) { _onLightsOffCommandReceived = callback; }

  bool isConnected() { return _mqttClient.connected(); }

  void loop() {
    if (WiFi.isConnected() && !_mqttClient.connected()) {
      _connectAndSubscribe();
      _onConnect();
      _connected = true;
    }

    if (_connected && !_mqttClient.connected()) {
      _onDisconnect();
      _connected = false;
    }

    if ((millis() - _pingTimeout) > MQTT_PING_INTERVAL && _mqttClient.connected()) {
      _mqttClient.publish((_deviceId + "/ping").c_str(), "");
      _pingTimeout = millis();
    }

    _mqttClient.loop();
  }
};
