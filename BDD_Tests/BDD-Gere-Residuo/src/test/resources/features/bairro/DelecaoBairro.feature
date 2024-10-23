# language: pt

@BAIRRO @FUNCIONAL @DELETE
Funcionalidade: Deleção de bairro
  Como ADMIN ou OPERADOR do sistema
  Quero excluir um bairro
  Para eventuais necessidades

  Contexto: Autenticação na API para obtenção do Token JWT
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                             |
      | usuarioEmail   | adminbaseteste@gereresiduo.com.br |
      | usuarioSenha   | Teste123@                         |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario/Login" de Login
    Então o status code esperado é o 200
    E o JSON Schema de validação a ser usado é o "Login de usuário bem sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado
    E o Token JWT seja recuperado da resposta da API
    Então o Token JWT retornado deve ser valido com a Secret Key
    Dado que eu tenha os seguintes dados de bairro:
      | atributo            | valor                          |
      | bairroNome          | Bairro das Flores              |
      | quantidadeLixeiras  | 1                              |
      | pesoMedioLixeirasKg | 120                            |
    Quando uma requisição POST for enviada para a rota "/api/v1/Bairro" de cadastro de bairro
    Então o status code que a API de cadastro de Bairro deve retornar é o 201
    E o JSON Schema de validação a ser usado contra a resposta da API de cadastro de Bairro é o "Cadastro de bairro bem-sucedido"
    Então a resposta da requisição da API de cadastro de Bairro deve estar em conformidade com o JSON Schema selecionado

  Cenário: Exclusão bem-sucedida de colaborador pelo ID
    Dado que eu recupere o ID do bairro criado
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Bairro" passando o ID do bairro como Path Parameter
    Então o status code que a API de cadastro de Bairro deve retornar é o 204

  @HOOK_CLEAN_BAIRRO_AFTER_SCENARIO
  Cenário: Exclusão mal-sucedida de bairro pelo ID
    Dado que eu especifique um ID de bairro invalido
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Bairro" passando o ID do bairro como Path Parameter
    Então o status code que a API de cadastro de Bairro deve retornar é o 404
    E a API de cadastro de Bairro deve retornar um objeto JSON contendo uma mensagem de erro: "O bairro de ID: 0 não existe!"