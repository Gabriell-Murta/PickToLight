import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/auth.model.dart';
import 'package:src/app/models/device.model.dart';
import 'package:src/app/models/product.model.dart';
import 'package:src/app/models/user.model.dart';
import 'package:src/app/notifiers/auth.notifier.dart';
import 'package:src/app/notifiers/device.notifier.dart';
import 'package:src/app/notifiers/product.notifier.dart';
import 'package:src/app/notifiers/user.notifier.dart';

final authProvider = StateNotifierProvider<AuthNotifier, AsyncValue<AuthResponse?>>((ref) => AuthNotifier());

final productProvider =
    StateNotifierProvider<ProductNotifier, AsyncValue<List<Product>>>((ref) => ProductNotifier(ref.read));

final deviceProvider =
    StateNotifierProvider<DeviceNotifier, AsyncValue<List<Device>>>((ref) => DeviceNotifier(ref.read));

final userProvider = StateNotifierProvider<UserNotifier, AsyncValue<User?>>((ref) => UserNotifier(ref.read));
