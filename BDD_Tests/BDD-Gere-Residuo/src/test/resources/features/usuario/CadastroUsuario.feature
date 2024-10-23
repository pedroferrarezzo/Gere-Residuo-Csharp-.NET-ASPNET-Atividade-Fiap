# language: pt

@USUARIO @FUNCIONAL @CREATE
Funcionalidade: Cadastro de novo usuário OPERADOR
  Como administrador de usuários
  Quero cadastrar um novo usuário OPERADOR
  Para que ele realize operações no sistema

  @HOOK_CLEAN_USUARIO_AFTER_SCENARIO
  Cenário: Cadastro de usuário bem-sucedido
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                          |
      | usuarioNome    | Operador BDD                   |
      | usuarioEmail   | operadorbdd@gereresiduo.com.br |
      | usuarioSenha   | Teste123@                      |
      | usuarioRole    | OPERADOR                       |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario" de cadastro de usuário
    Então o status code esperado é o 201
    E o JSON Schema de validação a ser usado é o "Cadastro de usuário bem-sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado

  Cenário: Cadastro de usuário mal-sucedido ao não passar um atributo obrigatório
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                          |
      | usuarioNome    | Operador BDD                   |
      | usuarioEmail   | operadorbdd@gereresiduo.com.br |
      | usuarioRole    | OPERADOR                       |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario" de cadastro de usuário
    Então o status code esperado é o 400
    E a API deve retornar um objeto JSON contendo uma mensagem de erro: "A senha do usuário é obrigatória!"