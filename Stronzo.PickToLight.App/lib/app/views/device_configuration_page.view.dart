import 'package:flutter/material.dart';
import 'package:flutter_blue_plus/flutter_blue_plus.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/shared/constants.dart';
import 'package:wifi_scan/wifi_scan.dart';

class DeviceConfigurationPage extends ConsumerStatefulWidget {
  final BluetoothDevice device;

  const DeviceConfigurationPage({Key? key, required this.device}) : super(key: key);

  @override
  ConsumerState<ConsumerStatefulWidget> createState() => _DeviceConfigurationPageState();
}

class _DeviceConfigurationPageState extends ConsumerState<DeviceConfigurationPage> {
  _showLoadingDialog() {
    showDialog(
      context: context,
      builder: (context) => AlertDialog(
        content: SizedBox.fromSize(
          size: const Size.square(200),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.center,
            children: const [
              CircularProgressIndicator(),
              SizedBox(height: 20),
              Text(
                "Estamos configurando seu dispositivo...",
                textAlign: TextAlign.center,
              )
            ],
          ),
        ),
      ),
    );
  }

  _showErrorDialog() {
    showDialog(
      context: context,
      builder: (context) => const Dialog(
        child: Text("Não conseguimos configurar seu dispositivo :("),
      ),
    );
  }

  _sendCredentialsToDevice(String ssid, String password) async {
    _showLoadingDialog();

    try {
      var btDevice = widget.device;

      var state = await btDevice.state.first;

      if (state == BluetoothDeviceState.disconnected) {
        await btDevice.connect();
      }

      await btDevice.requestMtu(517);

      var services = await btDevice.discoverServices();
      var pickToLightService =
          services.firstWhere((element) => element.uuid == BluetoothConstants.pickToLightServiceUuid);

      var credentialsCharacteristic = pickToLightService.characteristics
          .firstWhere((element) => element.uuid == BluetoothConstants.credentialsCharacteristicUuid);

      var credentialsJson = '{ "ssid": "$ssid", "password": "$password" }';

      await credentialsCharacteristic.write(credentialsJson.codeUnits);

      Navigator.pop(context);
    } catch (e) {
      Navigator.pop(context);
      _showErrorDialog();
    }
  }

  _showPasswordDialog(String ssid) {
    final _passwordInputController = TextEditingController();

    showDialog(
      context: context,
      barrierDismissible: false,
      builder: (BuildContext context) {
        return AlertDialog(
          actions: [
            ElevatedButton(
              onPressed: () {
                Navigator.pop(context);
                _sendCredentialsToDevice(ssid, _passwordInputController.text);
              },
              child: const Text("Send credentials"),
            )
          ],
          content: TextField(
            decoration: const InputDecoration(label: Text("Senha")),
            maxLength: 20,
            obscureText: true,
            controller: _passwordInputController,
          ),
        );
      },
    );
  }

  Widget _buildWifiApList() => StreamBuilder(
      stream: WiFiScan.instance.onScannedResultsAvailable,
      builder: (context, AsyncSnapshot<Result<List<WiFiAccessPoint>, GetScannedResultsErrors>> snapshot) {
        if (snapshot.hasData) {
          var aps = snapshot.data!.value!;

          return ListView.builder(
            itemCount: aps.length,
            itemBuilder: (context, index) => ListTile(
              onTap: () => _showPasswordDialog(aps[index].ssid),
              title: Text(aps[index].ssid),
            ),
          );
        }

        return const SizedBox.shrink();
      });

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text("Configuração de dispositivo"),
      ),
      body: RefreshIndicator(
        onRefresh: () => WiFiScan.instance.startScan(),
        child: _buildWifiApList(),
      ),
    );
  }
}
