#pragma once

#include <Arduino.h>
#include <WiFi.h>

#include "interfaces/helpers/ConfigHelper.h"
#include "setups/NetworkSetup.h"

class ConnectionHelper {
 private:
  bool _isWifiConnecting = false;
  bool _ledState = false;

  uint32_t _wifiTimeout = 0;

  std::function<void()> _onWifiConnectedCallback = []() {};

  void _handleWifiStationConnection() {
    if (configHelper.isConfigValid()) {
      if (!WiFi.isConnected() && !_isWifiConnecting) {
        setupWifiStation();

        _isWifiConnecting = true;
        _wifiTimeout = millis();
      }

      if (WiFi.isConnected() && _isWifiConnecting) {
        log_d("WiFi is connected! SSID: %s | IP: %s", WiFi.SSID().c_str(), WiFi.localIP().toString().c_str());

        _onWifiConnectedCallback();

        _isWifiConnecting = false;
      }

      if ((millis() - _wifiTimeout) > 10000UL && !WiFi.isConnected() && _isWifiConnecting) {
        log_w("WiFi connection timed out. Trying to reconnect...");

        _isWifiConnecting = false;
      }
    }
  }

 public:
  void setWifiConnectedCallback(std::function<void()> callback) { this->_onWifiConnectedCallback = callback; }

  void begin() { setupWifiStation(); }

  void loop() { _handleWifiStationConnection(); }
};
