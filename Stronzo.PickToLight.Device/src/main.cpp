#include <Arduino.h>
#include <ArduinoNvs.h>

#include "interfaces/clients/MqttClient.h"
#include "interfaces/helpers/BluetoothHelper.h"
#include "interfaces/helpers/ConfigHelper.h"
#include "interfaces/helpers/ConnectionHelper.h"
#include "interfaces/helpers/IndicationStateHelper.h"
#include "interfaces/helpers/InputHelper.h"

MqttClient* mqttClient;
ConnectionHelper connectionHelper;
InputHelper inputHelper;

void configLoop() {
  while (1) {
    if (restart)
      ESP.restart();

    indicationHelper.loop();
    bluetoothHelper.loop();

    delay(1);
  }
}

void setup() {
  NVS.begin();

  configHelper.begin();
  indicationHelper.begin();

  if (!configHelper.isConfigValid()) {
    indicationHelper.setState(IndicationState::Configuration);
    bluetoothHelper.begin(true);

    configLoop();
  } else {
    connectionHelper.begin();
    inputHelper.begin();
    bluetoothHelper.begin(false);
    indicationHelper.setState(IndicationState::Disconnected);

    mqttClient = new MqttClient(CONFIG_MQTT_ENDPOINT, configHelper.config.deviceId, configHelper.config.deviceToken);
    mqttClient->setOnConnectCallback([]() { indicationHelper.setState(IndicationState::Idle); });
    mqttClient->setOnDisconnectCallback([]() { indicationHelper.setState(IndicationState::Disconnected); });
    mqttClient->setOnLightOnCallback([](DeviceCommand command) {
      log_d(
          "LightOn command processed successfully: \nType: %d\nDeviceId: %s\nTimestamp: %s",
          command.type,
          command.deviceId.c_str(),
          command.timestamp.c_str());

      indicationHelper.setState(IndicationState::Active);
    });
    mqttClient->setOnLightOffCallback([](DeviceCommand command) {
      log_d(
          "LightOff command processed successfully: \nType: %d\nDeviceId: %s\nTimestamp: %s",
          command.type,
          command.deviceId.c_str(),
          command.timestamp.c_str());

      indicationHelper.setState(IndicationState::Idle);
    });
  }
}

void loop() {
  connectionHelper.loop();
  indicationHelper.loop();
  inputHelper.loop();
  mqttClient->loop();
}
