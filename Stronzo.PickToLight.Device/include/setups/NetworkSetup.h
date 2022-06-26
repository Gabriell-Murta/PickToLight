#pragma once

#include <Arduino.h>
#include <WiFi.h>

#include "interfaces/helpers/ConfigHelper.h"

void setupWiFiAP() {
  char apName[64];
  snprintf(apName, 64, "STRONZO_PTL_DVC-%llu", ESP.getEfuseMac());

  WiFi.mode(WIFI_AP);

  if (!WiFi.enableAP(true)) {
    log_e("Unable to enableAP");
  } else {
    log_d("EnabledAP");
  }
  if (!WiFi.softAPConfig(IPAddress(192, 168, 0, 1), IPAddress(0, 0, 0, 0), IPAddress(255, 255, 255, 0))) {
    log_e("SoftAP unable to config!");
  } else {
    log_d("SoftAP config'd. APIP: %s", WiFi.softAPIP().toString().c_str());
  }

  if (!WiFi.softAP(apName, "123456789", 11, false)) {
    log_e("SoftAP unable to init!");
  } else {
    log_d("SoftAP initialized. APIP: %s, SSID: %s, PASS: [REMOVED]", WiFi.softAPIP().toString().c_str(), apName);
  }
}

void setupWifiStation() {
  WiFi.mode(WIFI_STA);
  WiFi.begin(configHelper.config.wifiSsid, configHelper.config.wifiPass);
}
