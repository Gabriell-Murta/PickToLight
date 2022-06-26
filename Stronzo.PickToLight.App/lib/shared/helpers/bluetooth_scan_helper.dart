import 'package:flutter_blue_plus/flutter_blue_plus.dart';
import 'package:src/shared/constants.dart';

beginScan() => FlutterBluePlus.instance.startScan(
      timeout: const Duration(seconds: 4),
      // withServices: List.from({BluetoothConstants.pickToLightServiceUuid}),
    );

getResultsFilteredByConfigMode(bool enabled) =>
    FlutterBluePlus.instance.scanResults.map((results) => results.where((result) {
          var manData = result.advertisementData.manufacturerData;
          return manData[0x0C] != null && manData[0x0C]![0] == (enabled ? 1 : 0);
        }).toList());
