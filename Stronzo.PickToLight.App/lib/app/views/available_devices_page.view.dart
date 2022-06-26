import 'package:flutter/material.dart';
import 'package:flutter_blue_plus/flutter_blue_plus.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/views/available_devices_list.view.dart';
import 'package:src/shared/helpers/bluetooth_scan_helper.dart';

class AvailableDevicesPage extends ConsumerStatefulWidget {
  const AvailableDevicesPage({Key? key}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _AvailableDevicesPageState();
}

class _AvailableDevicesPageState extends ConsumerState<AvailableDevicesPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Dispositivos próximos disponíveis'),
        actions: const [],
      ),
      body: const AvailableDevicesList(unconfiguredDevices: true),
      floatingActionButton: StreamBuilder<bool>(
        stream: FlutterBluePlus.instance.isScanning,
        initialData: false,
        builder: (c, snapshot) {
          if (snapshot.data!) {
            return FloatingActionButton(
              child: const Icon(Icons.stop),
              onPressed: () => FlutterBluePlus.instance.stopScan(),
              backgroundColor: Colors.red,
            );
          } else {
            return FloatingActionButton(child: const Icon(Icons.search), onPressed: () => beginScan());
          }
        },
      ),
    );
  }
}
