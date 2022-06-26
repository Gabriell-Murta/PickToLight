import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/device.model.dart';
import 'package:src/shared/repository_base.dart';

class DeviceRepository extends ProviderRepositoryBase {
  DeviceRepository(Reader read) : super(read);

  Future<List<Device>> getDevicesByClientId(String clientId) async {
    var response = await dio.get<List<dynamic>>('/api/v1/devices?clientId=$clientId');

    if (response.statusCode! >= 200 && response.statusCode! < 300) {
      return response.data!.map((element) => Device.fromJson(element)).toList();
    }

    throw Exception('Não foi possível obter os produtos');
  }
}
