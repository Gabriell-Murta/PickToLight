import 'package:flutter/material.dart';
import 'package:hooks_riverpod/hooks_riverpod.dart';
import 'package:src/app/models/auth.model.dart';
import 'package:src/app/models/user.model.dart';
import 'package:src/app/providers/providers.dart';
import 'package:src/app/views/main.view.dart';
import 'package:src/shared/helpers/snack_bar_helper.dart';

class LoginPage extends ConsumerWidget {
  LoginPage({Key? key}) : super(key: key);

  final _loginFieldController = TextEditingController.fromValue(const TextEditingValue(text: 'teste@gmail.com'));
  final _passwordFieldController = TextEditingController.fromValue(const TextEditingValue(text: '123456'));
  final _formKey = GlobalKey<FormState>();

  String? _validateLogin(String? text) {
    final isValidEmail = RegExp(
        r"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$");

    if (text == null) return 'O e-mail é obrigatório.';
    if (text.isEmpty) return 'O e-mail é obrigatório.';
    if (!isValidEmail.hasMatch(text)) return 'Formato de e-mail inválido.';

    return null;
  }

  String? _validatePassword(String? text) {
    final isAlphanumeric = RegExp(r'^[a-zA-Z0-9]+$');

    if (text == null) return 'A senha é obrigatória.';
    if (text.isEmpty) return 'A senha é obrigatória.';
    if (!isAlphanumeric.hasMatch(text)) {
      return 'A senha só pode conter caracteres alfanuméricos';
    }

    return null;
  }

  _buildEmailField() => TextFormField(
        controller: _loginFieldController,
        validator: _validateLogin,
        decoration: const InputDecoration(
          labelText: "Email",
          hintText: "Informe o e-mail",
          labelStyle: TextStyle(fontSize: 20.0),
        ),
      );

  _buildPasswordField() => TextFormField(
        obscureText: true,
        controller: _passwordFieldController,
        validator: _validatePassword,
        decoration: const InputDecoration(
          labelText: "Senha",
          hintText: "Informe a senha",
          labelStyle: TextStyle(fontSize: 20.0),
        ),
      );

  _buildButtonContainer(Function pressed, bool isLoading) => Container(
        height: 40.0,
        margin: const EdgeInsets.only(top: 10.0),
        child: isLoading
            ? const Center(child: CircularProgressIndicator())
            : ElevatedButton(
                clipBehavior: Clip.antiAlias,
                onPressed: () {
                  pressed();
                },
                child: const Text("Login"),
              ),
      );

  @override
  Widget build(BuildContext context, WidgetRef ref) {
    final authState = ref.watch(authProvider);
    final userState = ref.watch(userProvider);

    _gotoMainPage() => Navigator.pushReplacement(context, MaterialPageRoute(builder: (context) => const MainPage()));

    ref.listen<AsyncValue<AuthResponse?>>(authProvider, (_, state) {
      state.whenOrNull(
        loading: () {},
        data: (value) {
          if (value != null) {
            ref.read(userProvider.notifier).getUserById(value.userId);
          }
        },
        error: (error, _) => showErrorSnackBar(context, error),
      );
    });

    ref.listen<AsyncValue<User?>>(userProvider, (_, state) {
      state.whenOrNull(
        loading: () {},
        data: (value) {
          if (value != null) {
            ref.read(deviceProvider.notifier).getDevices(value.clientId);
            ref.read(productProvider.notifier).getProducts(value.clientId);

            _gotoMainPage();
          }
        },
        error: (error, _) => showErrorSnackBar(context, error),
      );
    });

    Future<void> _handleLogin() async {
      final email = _loginFieldController.text;
      final password = _passwordFieldController.text;

      if (!_formKey.currentState!.validate()) {
        return;
      }

      if (!authState.isLoading) {
        if (authState.hasValue) {
          ref.refresh(authProvider);
        }

        ref.read(authProvider.notifier).authenticate(email, password);
      }
    }

    return Scaffold(
      appBar: AppBar(
        title: const Center(
          child: Text("STRONZO LTDA PICKTOLIGHT"),
        ),
      ),
      body: Center(
        child: Form(
          key: _formKey,
          autovalidateMode: AutovalidateMode.onUserInteraction,
          child: ListView(
            padding: const EdgeInsets.all(20.0),
            children: [
              _buildEmailField(),
              _buildPasswordField(),
              _buildButtonContainer(_handleLogin, authState.isLoading || userState.isLoading),
            ],
          ),
        ),
      ),
    );
  }
}
