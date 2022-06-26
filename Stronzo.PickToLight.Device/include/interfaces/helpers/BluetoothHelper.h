#pragma once

#include <Arduino.h>

#include <BLEAdvertising.h>

#include "ConfigHelper.h"

#include "interfaces/server/controllers/CredentialsController.h"
#include "interfaces/server/controllers/HighlightController.h"
#include "utils/BluetoothUtils.h"

class BluetoothHelper {
 private:
  bool _hasDeviceConnected = false;
  bool _oldDeviceConnected = false;

  uint32_t _pollingMillis = 0;

  void _initializeBle() {
    auto serverCallbacks = new PickToLightBleServerCallbacks(
        [this]() { this->handleDeviceConnected(); }, [this]() { this->handleDeviceDisconnected(); });

    BLEDevice::init("");
    BLEDevice::setMTU(517);
    bleServer = BLEDevice::createServer();
    bleServer->setCallbacks(serverCallbacks);
  }

  void _initializeService(bool isConfig) {
    BLEService* pickToLightService = bleServer->createService(PICKTOLIGHT_SERVICE_UUID);

    if (isConfig) {
      BLECharacteristic* bleCredentialsCharacteristic =
          pickToLightService->createCharacteristic(CREDENTIALS_CHARACTERISTIC_UUID, BLECharacteristic::PROPERTY_WRITE);

      auto credentialCallbacks = new CredentialsCharacteristicCallbacks(CredentialsController::setCredentials);
      bleCredentialsCharacteristic->setCallbacks(credentialCallbacks);
    }

    BLECharacteristic* bleIdCharacteristic =
        pickToLightService->createCharacteristic(ID_CHARACTERISTIC_UUID, BLECharacteristic::PROPERTY_READ);
    bleIdCharacteristic->setValue(configHelper.config.deviceId);

    BLECharacteristic* bleHighlightCharacteristic =
        pickToLightService->createCharacteristic(HIGHLIGHT_CHARACTERISTIC_UUID, BLECharacteristic::PROPERTY_READ);
    auto highlightCallbacks = new HighlightCharacteristicCallbacks(HighlightController::highlight);
    bleHighlightCharacteristic->setCallbacks(highlightCallbacks);

    pickToLightService->start();
  }

  void _initializeAdvertising(bool isConfig) {
    BLEAdvertising* bleAdvertising = bleServer->getAdvertising();

    BLEAdvertisementData advData = BLEAdvertisementData();
    advData.setName("");
    advData.setCompleteServices(BLEUUID(PICKTOLIGHT_SERVICE_UUID));

    BLEAdvertisementData scanResponseData = BLEAdvertisementData();
    scanResponseData.setName("");
    scanResponseData.setManufacturerData(string_format("%c%c%c", 0x0C, 0, isConfig));

    bleAdvertising->setAdvertisementData(advData);

    bleAdvertising->setScanResponseData(scanResponseData);
    bleAdvertising->setScanResponse(true);

    bleAdvertising->addServiceUUID(BLEUUID(PICKTOLIGHT_SERVICE_UUID));
    bleAdvertising->start();

    log_d("BLE configured.");
  }

 public:
  void handleDeviceDisconnected() { _hasDeviceConnected = false; }

  void handleDeviceConnected() { _hasDeviceConnected = true; }

  void stop() {
    BLEDevice::deinit();
    BLEDevice::stopAdvertising();
  }

  void begin(bool isConfig) {
    _initializeBle();
    _initializeService(isConfig);
    _initializeAdvertising(isConfig);
  }

  void loop() {
    if ((millis() - _pollingMillis) > 100) {
      if (_hasDeviceConnected) {
        delay(10);
      }

      if (!_hasDeviceConnected && _oldDeviceConnected) {
        bleServer->startAdvertising();
        _oldDeviceConnected = _hasDeviceConnected;
      }

      if (_hasDeviceConnected && !_oldDeviceConnected) {
        _oldDeviceConnected = _hasDeviceConnected;
      }

      _pollingMillis = millis();
    }
  }
};

static BluetoothHelper bluetoothHelper;
