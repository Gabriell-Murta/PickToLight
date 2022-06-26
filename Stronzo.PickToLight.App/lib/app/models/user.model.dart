class User {
  final String id;
  final String name;
  final String email;
  final String clientId;

  User({required this.id, required this.name, required this.email, required this.clientId});

  @override
  String toString() => 'User { id: $id, name: $name, email: $email, clientId: $clientId }';

  factory User.fromJson(Map<String, dynamic> json) =>
      User(id: json['id'], name: json['name'], email: json['email'], clientId: json['clientId']);

  Map<String, dynamic> toJson() => <String, dynamic>{'id': id, 'name': name, 'email': email, 'clientId': clientId};
}
