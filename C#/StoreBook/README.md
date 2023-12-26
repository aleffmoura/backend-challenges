
# Notas adicionais
Devido às características da linguagem C#, a implementação da imutabilidade no estilo de programação orientada a objetos pode ser um pouco complicada.

`existe um arquivo BookStore.postman_collection.json que pode ser importado para o postman para testes caso não queira usar swagger`

#### Sei que utilizei muita coisa e o projeto ficou grande mesmo sendo uma situação simples, contudo, dei meu sangue pois as vezes é necessario mostrar nosso estilo de programar, quero deixar claro que sou modular e consigo me adaptar a qualquer estilo necessario para desenvolvimento.
#### Foi evitada muita coisa para não deixa o projeto complexo alem do necessario.


#### O metodo de entrada `PATCH` foi deixado para atualizar a capa do livro, tendo em vista que muda apenas uma propriedade da entidade, foi criado um metodo PUT para atualizar o livro, tendo em vista que ele modifica o objeto por completo, conforme dita o RESTfull.

# Técnicas e Arquitetura

Este projeto foi desenvolvido utilizando uma estrutura simples baseada na abordagem "folder by feature" e em uma arquitetura em camadas.

Foi adotada uma abordagem combinada de Orientação a Objetos e técnicas de programação funcional.
No entanto, foi evitado um maior enfoque na programação funcional devido à natureza verbosa do C# para esse tipo de abordagem.

Algumas técnicas aplicadas incluem: Funções de primeira classe, funções em primeira ordem, Funções Anônimas (Lambda), Currying, Partial Application.

# Frameworks/Bibliotecas

Foram utilizadas as seguintes ferramentas:
- FluentValidation: para validação das requisições HTTP em forma de middleware.
- MediatR: para CQRS e funcionamento assincrono para mensagens disparadas internamente(no caso do envio da capa do livro).
- Swagger: para open api, e funcionamento de envio de requisições.
- Autofac: para gerenciamento de Injeção de dependencias.
- AutoMapper: para mapeamento de entidades dto e view model.
- AWS S3: para envio dos bytes de imagem para o amazon.
- Newtonsoft.Json: Para manipulação dos inputs inseridos pelo usuário.
- EntityFrameworkCore: para programação code first em sql server.
- FluentAssertions: Oferecendo uma abordagem mais fluente ("fluent-api") para verificação dos casos de teste.
- Moq: Utilizado nos testes.
- NUnit: Escolhido para a execução dos testes.

## Execução
Necessario realizar as modificações necessarias em: `appsettings.json` menos o IP pois ele é do docker gerado no docker compose.

Utilizando visual studio utilizar do projeto docker-compose para subir a aplicação e realizar os testes, a aplicação a principio fara a aplicação de criação do banco e aplicação das migrações.

Caso por alguma razão não funcione, necessario aplicar o migration do banco de dados utilizando o entity framework, não esquecer de modificar a conection string em `BookStoreContextFactoryDesignTime` para o seu endereço de ip. na migration manual é necessaria a mudannça, porem na migration automatica ele pega com base no IP configurado no docker que esta setado em `appsettings.json`.

### Testes
Existem dois projetos de testes:
`ManagementBook.Application.Tests` e `ManagementBook.Infra.Data.Tests` basta executar com o visual studio, contudo pode ser realizado via linha de comando.

```bash
dotnet test *.sln
```