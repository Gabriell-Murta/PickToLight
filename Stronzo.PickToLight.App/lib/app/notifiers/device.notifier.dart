import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/device.model.dart';
import 'package:src/app/providers/repository_providers.dart';

class DeviceNotifier extends StateNotifier<AsyncValue<List<Device>>> {
  final Reader read;

  DeviceNotifier(this.read) : super(AsyncValue.data(List.empty()));

  Future<void> getDevices(String clientId) async {
    try {
      state = const AsyncValue.loading();
      state = AsyncValue.data(await read(deviceRepositoryProvider).getDevicesByClientId(clientId));
    } catch (e) {
      state = AsyncValue.error(e.toString());
    }
  }
}
