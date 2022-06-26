#pragma once
#include <Arduino.h>
#include <ArduinoJson.h>
#include <LittleFS.h>

#include "entities/Config.h"
#include "utils/ProjUtils.h"

class ConfigHelper {
 private:
  StaticJsonDocument<CONFIG_DOC_SIZE> _jsonDocument;
  std::string _configDir;
  bool _hasConfig = false;

 public:
  Config config;

  explicit ConfigHelper(const std::string& filename) {
    this->_configDir = filename;
    this->config = Config();
  }

  bool begin() {
    if (!LittleFS.begin()) {
      log_e("Unable to initialize LittleFS in Config class.");
      return false;
    }

    if (!loadConfig()) {
      this->_hasConfig = false;
    } else {
      this->_hasConfig = true;
    }

    log_d("Parsed config!");

    return true;
  }

  bool hasConfig() { return this->_hasConfig; }

  bool isConfigValid() { return this->config.isConfigValid; }

  bool loadConfig() {
    File file = LittleFS.open(this->_configDir.c_str(), "r");
    DeserializationError err = deserializeJson(_jsonDocument, file);

    if (err) {
      log_d("Failed to read json file. Using default config. Error: %s", err.c_str());
      file.close();
      return false;
    }

    this->config = _jsonDocument.as<Config>();

    file.close();
    _jsonDocument.clear();

    return true;
  }

  bool saveConfig() {
    LittleFS.remove(this->_configDir.c_str());
    File file = LittleFS.open(this->_configDir.c_str(), "w");

    if (!file) {
      log_e("Failed to create config file! Possible flash error.");

      LittleFS.remove(this->_configDir.c_str());
      file.close();

      return false;
    }

    _jsonDocument.set(this->config);

    if (serializeJson(_jsonDocument, file) == 0) {
      log_e("Serialize failed. JSON error.");
    }

    file.close();
    _jsonDocument.clear();

    return true;
  }

  bool deleteConfig() { return LittleFS.remove(CONFIG_DIR); }

  void printConfig() {
    Serial.println("\n//----------------------------------------//");
    Serial.println("Config file found! Here's the loaded configs:");

    _jsonDocument.set(config);
    serializeJsonPretty(_jsonDocument, Serial);
    _jsonDocument.clear();

    Serial.println("\n//----------------------------------------//\n");
  }
};

static ConfigHelper configHelper(CONFIG_DIR);
