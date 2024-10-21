# language: pt

@USUARIO @FUNCIONAL
Funcionalidade: Deleção de usuário
  Como administrador de usuários
  Quero excluir um usuário
  Para que ele não realize mais operações no sistema

  Contexto: Cadastro de usuário bem-sucedido
    Dado que eu tenha os seguintes dados de usuário:
      | atributo       | valor                        |
      | usuarioNome    | Admin Teste                  |
      | usuarioEmail   | adminteste@exemplo.com.br    |
      | usuarioSenha   | Teste123@                    |
      | usuarioRole    | ADMIN                        |
    Quando uma requisição POST for enviada para a rota "/api/v1/Usuario" de cadastro de usuário
    Então o status code esperado é o 201
    E o JSON Schema de validação a ser usado é o "Cadastro de usuário bem-sucedido"
    Então a resposta da requisição deve estar em conformidade com o JSON Schema selecionado

  Cenário: Exclusão bem-sucedida de colaborador pelo ID
    Dado que eu recupere o ID do usuário criado durante a execução do contexto
    Quando uma requisição DELETE for enviada para a rota "/api/v1/Usuario" passando o ID do usuário como Path Parameter
    Então o status code esperado é o 200