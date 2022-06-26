import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/user.model.dart';
import 'package:src/app/providers/repository_providers.dart';

class UserNotifier extends StateNotifier<AsyncValue<User?>> {
  final Reader read;

  UserNotifier(this.read) : super(const AsyncValue.data(null));

  Future<void> getUserById(String userId) async {
    try {
      state = const AsyncValue.loading();
      state = AsyncValue.data(await read(userRepositoryProvider).getUserById(userId));
    } catch (e) {
      state = AsyncValue.error(e.toString());
    }
  }
}
