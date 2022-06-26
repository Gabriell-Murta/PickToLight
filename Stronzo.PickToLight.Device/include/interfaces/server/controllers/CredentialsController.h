#pragma once

#include <Arduino.h>
#include <ArduinoJson.h>

#include "interfaces/helpers/ConfigHelper.h"

#include "utils/BluetoothUtils.h"
#include "utils/ProjUtils.h"

class CredentialsController {
 public:
  static SetCredentialsHandler setCredentials;
};

SetCredentialsHandler CredentialsController::setCredentials = [](CredentialsRequest request) {
  log_d("Received setCredentials request");

  strlcpy(configHelper.config.wifiSsid, request.ssid.c_str(), sizeof(configHelper.config.wifiSsid) - 1);
  strlcpy(configHelper.config.wifiPass, request.password.c_str(), sizeof(configHelper.config.wifiPass) - 1);

  configHelper.config.isConfigValid = true;
  
  configHelper.saveConfig();

  restart = true;
};
