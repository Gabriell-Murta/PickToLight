#pragma once

#include <BLEDevice.h>
#include <BLEUtils.h>

#include "dtos/CredentialsRequest.h"

#define PICKTOLIGHT_SERVICE_UUID "9e98d7af-d2f9-42f5-acd2-bcb5a5cdc7df"

#define CREDENTIALS_CHARACTERISTIC_UUID "0c8b055e-919a-4cb6-b5af-f7919680da16"
#define ID_CHARACTERISTIC_UUID "0eacdcfd-95f1-46b0-bdd8-367b8a5d4d54"
#define HIGHLIGHT_CHARACTERISTIC_UUID "21aea235-3ed6-46f7-880d-5125cd75ebc4"

static BLEServer* bleServer;

typedef std::function<void()> OnConnectHandler;
typedef std::function<void()> OnDisconnectHandler;
typedef std::function<void(CredentialsRequest)> SetCredentialsHandler;
typedef std::function<void()> HighlightHandler;

class PickToLightBleServerCallbacks : public BLEServerCallbacks {
 private:
  OnConnectHandler _onConnectCallback;
  OnDisconnectHandler _onDisconnectCallback;

 public:
  PickToLightBleServerCallbacks(OnConnectHandler connectCallback, OnDisconnectHandler disconnectCallback)
      : _onConnectCallback(connectCallback), _onDisconnectCallback(disconnectCallback) {}

  void onConnect(BLEServer* pServer) {
    log_d(
        "Client Connected! Peer MTU: %d | Conn ID: %d",
        pServer->getPeerMTU(pServer->getConnId()),
        pServer->getConnId());
    BLEDevice::startAdvertising();

    _onConnectCallback();
  }

  void onDisconnect(BLEServer* pServer) {
    log_d(
        "Client Disconnected! Peer MTU: %d | Conn ID: %d",
        pServer->getPeerMTU(pServer->getConnId()),
        pServer->getConnId());

    _onDisconnectCallback();
  }
};

class CredentialsCharacteristicCallbacks : public BLECharacteristicCallbacks {
 private:
  SetCredentialsHandler _setCredentialsHandlerCallback;

  void onWrite(BLECharacteristic* characteristic) {
    log_i("Received on BLE: %s | Length: %d", characteristic->getValue().c_str(), characteristic->getLength());

    StaticJsonDocument<1024> jsonDoc;
    DeserializationError err = deserializeJson(jsonDoc, characteristic->getData(), 1024);

    if (!err) {
      _setCredentialsHandlerCallback(jsonDoc.as<CredentialsRequest>());
    } else {
      log_w("Received invalid credentials request %s", err.c_str());
    }
  }

 public:
  explicit CredentialsCharacteristicCallbacks(SetCredentialsHandler credentialsHandler)
      : _setCredentialsHandlerCallback(credentialsHandler) {}
};

class HighlightCharacteristicCallbacks : public BLECharacteristicCallbacks {
 private:
  HighlightHandler _highlightRequestCallback;

  void onRead(BLECharacteristic* characteristic, esp_ble_gatts_cb_param_t* param) { _highlightRequestCallback(); }

 public:
  explicit HighlightCharacteristicCallbacks(HighlightHandler handler) : _highlightRequestCallback(handler) {}
};
