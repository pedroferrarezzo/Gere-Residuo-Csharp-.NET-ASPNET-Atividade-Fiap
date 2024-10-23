# language: pt

@USUARIO @SMOKE @AUTH
Funcionalidade: Autenticação de usuário
  Como usuário
  Quero autenticar no sistema
  Para obter um Token JWT e realizar operações

  Contexto: Cadastro de usuário bem-sucedido
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                      |
      | usuarioNome    | User BDD                   |
      | usuarioEmail   | userbdd@gereresiduo.com.br |
      | usuarioSenha   | Teste123@                  |
      | usuarioRole    | USER                       |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario" de cadastro de usuário
    Então o status code esperado é o 201
    E o JSON Schema de validação a ser usado é o "Cadastro de usuário bem-sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado

  @HOOK_CLEAN_USUARIO_AFTER_SCENARIO
  Cenário: Autenticação bem-sucedida de usuário
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                      |
      | usuarioEmail   | userbdd@gereresiduo.com.br |
      | usuarioSenha   | Teste123@                  |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario/Login" de Login
    Então o status code esperado é o 200
    E o JSON Schema de validação a ser usado é o "Login de usuário bem sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado
    E o Token JWT seja recuperado da resposta da API
    Então o Token JWT retornado deve ser valido com a Secret Key

  @HOOK_CLEAN_USUARIO_AFTER_SCENARIO
  Cenário: Autenticação mal-sucedida de usuário
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                      |
      | usuarioEmail   | userbdd@gereresiduo.com.br |
      | usuarioSenha   | Teste1234567@              |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario/Login" de Login
    Então o status code esperado é o 401
    E a API deve retornar um objeto JSON contendo uma mensagem de erro: "Email ou senha incorretos!"