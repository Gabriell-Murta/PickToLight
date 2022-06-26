import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/user.model.dart';
import 'package:src/shared/repository_base.dart';

class UserRepository extends ProviderRepositoryBase {
  UserRepository(Reader read) : super(read);

  Future<User> getUserById(String id) async {
    var response = await dio.get('/api/v1/users/$id');

    if (response.statusCode! >= 200 && response.statusCode! < 300) {
      return User.fromJson(response.data!);
    }

    throw Exception('Não foi possível obter seu usuário :(');
  }
}
