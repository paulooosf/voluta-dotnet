<h1 align="center">Voluta</h1>
<h4 align="center">Projeto Voluta, um sistema de gerenciamento de Ongs e Voluntários desenvolvido usando o padrão MVVM.</h4>
<h5 align="center"><i>Desenvolvido para o curso de Análise e Desenvolvimento de Sistemas da FIAP</i></h5>
<p align="center">
  <img alt="C#" src="https://img.shields.io/badge/C%23-9b4993?style=for-the-badge&labelColor=%239b4993&color=%239b4993">
  <img alt=".NET" src="https://img.shields.io/badge/.net-%23512BD4?style=for-the-badge&logo=.net&logoColor=%23512BD4&labelColor=black">
  <img alt="JWT" src="https://img.shields.io/badge/jwt-000000?style=for-the-badge&logo=jsonwebtokens&labelColor=000000">
</p>
<p align="center">
  <a href="#funcionalidades">Funcionalidades</a> •
  <a href="#como-usar">Como usar</a> •
  <a href="#créditos">Créditos</a>
</p>

## Funcionalidades
- Busca, Listagem, Cadastro, Edição e Remoção de Usuários, Ongs e Solicitações de Voluntariado;
- Autenticação e Autorização;
- Exceções personalizadas e tratamento completo de erros;
- Testes unitários para Controllers e Serviços;
- Um usuário poderá solicitar à uma ong e se tornar um voluntário ligado a ela;
- Um representante da Ong poderá analisar os pedidos de voluntariado, podendo aprovar ou reprovar;
- Representantes de Ongs poderão ter acesso a listagem de voluntários de cada ong e de usuários disponíveis para voluntariar.
## Como usar
Certifique-se de ter o .NET 8.0 instalado.
1. Clone o repositório;
2. Acesse a pasta Voluta e abra o terminal, aplique as migrações ao banco de dados:
```
dotnet ef database update
```
3. Execute o projeto:
```
dotnet run
```
Após a inicialização, você poderá acessar todos os endpoints no Swagger em http://localhost:5177/swagger ou importar a coleção de requisições do Postman.

_OBS: O sistema pode ser inicializado em outra porta, verifique o terminal._

_OBS²: O sistema será inicializado com três usuários disponíveis: Um Usuário, um Representante e um Admin, confira suas credenciais na coleção do Postman._

## Créditos
- Paulo Henrique - [paulooosf](http://github.com/paulooosf)
