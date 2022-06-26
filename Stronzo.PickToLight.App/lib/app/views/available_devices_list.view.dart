import 'package:flutter/material.dart';
import 'package:flutter_blue_plus/flutter_blue_plus.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/views/device_configuration_page.view.dart';
import 'package:src/shared/helpers/bluetooth_scan_helper.dart';
import 'package:src/shared/widgets/scan_result_tile.widget.dart';

class AvailableDevicesList extends ConsumerStatefulWidget {
  final bool unconfiguredDevices;

  const AvailableDevicesList({required this.unconfiguredDevices, Key? key}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _AvailableDevicesListState();
}

class _AvailableDevicesListState extends ConsumerState<AvailableDevicesList> {
  _buildFoundDevices(BuildContext context) {
    return StreamBuilder<List<ScanResult>>(
      stream: getResultsFilteredByConfigMode(widget.unconfiguredDevices),
      builder: (c, snapshot) {
        if (!snapshot.hasData) return const SizedBox.shrink();

        if (snapshot.data!.isEmpty) {
          return Center(
            child: Text("Nenhum dispositivo encontrado :(\n(Já verificou se seu dispositivo está em modo de " +
                (widget.unconfiguredDevices ? "configuração?" : "vínculo?") +
                ")"),
          );
        }

        return Column(
          children: snapshot.data!
              .map(
                (scanResult) => ScanResultTile(
                  result: scanResult,
                  onTap: () {
                    Navigator.push(
                      context,
                      MaterialPageRoute(builder: (context) => DeviceConfigurationPage(device: scanResult.device)),
                    );
                  },
                ),
              )
              .toList(),
        );
      },
    );
  }

  @override
  Widget build(BuildContext context) {
    return SingleChildScrollView(child: _buildFoundDevices(context));
  }
}
