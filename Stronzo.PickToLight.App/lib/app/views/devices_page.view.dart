import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/views/devices_list.view.dart';

class DevicesPage extends ConsumerStatefulWidget {
  const DevicesPage({Key? key}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _DevicesPageState();
}

class _DevicesPageState extends ConsumerState<DevicesPage> {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Seus dispositivos registrados'),
        actions: const [],
      ),
      body: const DevicesList(),
    );
  }
}
