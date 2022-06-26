import 'package:flutter/material.dart';
import 'package:flutter_blue_plus/flutter_blue_plus.dart';
import 'package:src/shared/constants.dart';

class ScanResultTile extends StatefulWidget {
  const ScanResultTile({Key? key, required this.result, required this.onTap}) : super(key: key);

  final ScanResult result;
  final VoidCallback onTap;

  @override
  State<ScanResultTile> createState() => _ScanResultTileState();
}

class _ScanResultTileState extends State<ScanResultTile> {
  bool _isHighlighting = false;

  Widget _buildTitle(BuildContext context) => widget.result.device.name.isNotEmpty
      ? Column(
          mainAxisAlignment: MainAxisAlignment.start,
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text(
              widget.result.device.name,
              overflow: TextOverflow.ellipsis,
            ),
            Text(
              widget.result.device.id.toString(),
              style: Theme.of(context).textTheme.caption,
            )
          ],
        )
      : Text(widget.result.device.id.toString());

  Widget _buildAdvRow(BuildContext context, String title, String value) => Padding(
        padding: const EdgeInsets.symmetric(horizontal: 16.0, vertical: 4.0),
        child: Row(
          crossAxisAlignment: CrossAxisAlignment.start,
          children: <Widget>[
            Text(title, style: Theme.of(context).textTheme.caption),
            const SizedBox(
              width: 12.0,
            ),
            Expanded(
              child: Text(
                value,
                style: Theme.of(context).textTheme.caption!.apply(color: Colors.black),
                softWrap: true,
              ),
            ),
          ],
        ),
      );

  String getNiceHexArray(List<int> bytes) {
    return '[${bytes.map((i) => i.toRadixString(16).padLeft(2, '0')).join(', ')}]'.toUpperCase();
  }

  String getNiceManufacturerData(Map<int, List<int>> data) {
    if (data.isEmpty) {
      return 'N/A';
    }

    return data.entries
        .map((manufacturerData) =>
            '${manufacturerData.key.toRadixString(16).toUpperCase()}: ${getNiceHexArray(manufacturerData.value)}')
        .join(', ');
  }

  String getNiceServiceData(Map<String, List<int>> data) {
    if (data.isEmpty) {
      return 'N/A';
    }

    return data.entries.map((service) => '${service.key.toUpperCase()}: ${getNiceHexArray(service.value)}').join(', ');
  }

  _highlightDevice(ScanResult result) async {
    setState(() {
      _isHighlighting = true;
    });

    try {
      await result.device.connect(autoConnect: false);

      var services = await result.device.discoverServices();
      var ptlService = services.firstWhere((service) => service.uuid == BluetoothConstants.pickToLightServiceUuid);
      var highlightCharacteristic = ptlService.characteristics
          .firstWhere((characteristic) => characteristic.uuid == BluetoothConstants.highlightCharacteristicUuid);

      highlightCharacteristic.read();
    } catch (e) {
    } finally {
      await result.device.disconnect();

      setState(() {
        _isHighlighting = false;
      });
    }
  }

  @override
  Widget build(BuildContext context) {
    return ExpansionTile(
      title: _buildTitle(context),
      leading: _isHighlighting
          ? const CircularProgressIndicator()
          : IconButton(
              onPressed: () => _highlightDevice(widget.result),
              icon: const Icon(Icons.lightbulb),
            ),
      trailing: ElevatedButton(
        child: const Text('CONNECT'),
        onPressed: (widget.result.advertisementData.connectable) ? widget.onTap : null,
      ),
      children: <Widget>[
        _buildAdvRow(context, 'Complete Local Name', widget.result.advertisementData.localName),
        _buildAdvRow(context, 'Tx Power Level', '${widget.result.advertisementData.txPowerLevel ?? 'N/A'}'),
        _buildAdvRow(
            context, 'Manufacturer Data', getNiceManufacturerData(widget.result.advertisementData.manufacturerData)),
        _buildAdvRow(
            context,
            'Service UUIDs',
            (widget.result.advertisementData.serviceUuids.isNotEmpty)
                ? widget.result.advertisementData.serviceUuids.join(', ').toUpperCase()
                : 'N/A'),
        _buildAdvRow(context, 'Service Data', getNiceServiceData(widget.result.advertisementData.serviceData)),
      ],
    );
  }
}
