class AuthResponse {
  String userId = '';
  String accessToken = '';

  AuthResponse({required this.userId, required this.accessToken});

  factory AuthResponse.fromJson(Map<String, dynamic> json) =>
      AuthResponse(userId: json['userId'], accessToken: json['token']);

  Map<String, dynamic> toJson() => <String, dynamic>{'userId': userId, 'token': accessToken};
}
