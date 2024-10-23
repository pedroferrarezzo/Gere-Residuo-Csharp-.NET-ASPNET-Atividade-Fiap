# language: pt

@USUARIO @FUNCIONAL @DELETE
Funcionalidade: Deleção de usuário
  Como administrador de usuários
  Quero excluir um usuário
  Para que ele não realize mais operações no sistema

  Contexto: Cadastro de usuário bem-sucedido
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                         |
      | usuarioNome    | Admin BDD                     |
      | usuarioEmail   | adminbdd@gereresiduo.com.br   |
      | usuarioSenha   | Teste123@                     |
      | usuarioRole    | ADMIN                         |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario" de cadastro de usuário
    Então o status code esperado é o 201
    E o JSON Schema de validação a ser usado é o "Cadastro de usuário bem-sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado
    E que eu recupere o ID do usuário criado
    Então uma requisição POST for enviada para a rota "/api/v1/Usuario/Login" de Login
    E o status code esperado é o 200
    Então o JSON Schema de validação a ser usado é o "Login de usuário bem sucedido"
    E a resposta da requisição deve estar em conformidade com o JSON Schema selecionado
    Então o Token JWT seja recuperado da resposta da API
    E o Token JWT retornado deve ser valido com a Secret Key

  Cenário: Exclusão bem-sucedida de usuario pelo ID
    Dado que eu recupere o ID do usuário criado
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Usuario" passando o ID do usuário como Path Parameter
    Então o status code esperado é o 204

  @HOOK_CLEAN_USER_AFTER_SCENARIO
  Cenário: Exclusão mal-sucedida de usuario pelo ID
    Dado que eu especifique um ID de usuário invalido
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Usuario" passando o ID do usuário como Path Parameter
    Então o status code esperado é o 404
    E a API deve retornar um objeto JSON contendo uma mensagem de erro: "O usuário de ID: 0 não existe!"