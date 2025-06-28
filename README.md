# LoginUsuario API

Uma API REST completa para autenticação e gerenciamento de usuários, desenvolvida em C# .NET 8 com arquitetura limpa (Clean Architecture).

## 🎯 Objetivo

Este projeto foi desenvolvido com o objetivo de aprender e praticar a construção de uma API de autenticação completa, implementando:

- Cadastro de usuários
- Sistema de login com autenticação JWT
- Validação de tokens de acesso
- Arquitetura limpa com separação de responsabilidades
- Segurança com criptografia de senhas

## 🚀 Funcionalidades

### 🔐 Autenticação
- **Registro de usuários**: Criação de conta com validação de dados
- **Login**: Autenticação com email e senha
- **Tokens JWT**: Geração de tokens de acesso com duração de 1 minuto
- **Validação de sessão**: Verificação de autenticação e status do token

### 🛡️ Segurança
- Criptografia de senhas com BCrypt
- Tokens JWT seguros
- Validação de dados com FluentValidation
- Tratamento de exceções personalizado

## 🏗️ Arquitetura

O projeto segue os princípios da **Clean Architecture** com as seguintes camadas:

```
LoginUsuario/
├── 📁 LoginUsuario.Api/            # Camada de apresentação (Controllers)
├── 📁 LoginUsuario.Application/    # Casos de uso e regras de negócio
├── 📁 LoginUsuario.Domain/         # Entidades e interfaces do domínio
├── 📁 LoginUsuario.Infrastructure/ # Implementações (BD, JWT, Criptografia)
├── 📁 LoginUsuario.Comunication/   # DTOs de Request/Response
└── 📁 LoginUsuario.Exception/      # Exceções personalizadas
```

### Estrutura Detalhada

- **Api**: Controllers, configuração da aplicação e middleware
- **Application**: Use Cases, validadores e regras de negócio
- **Domain**: Entidades, interfaces e contratos
- **Infrastructure**: Repositórios, contexto do banco, serviços de segurança
- **Communication**: DTOs para comunicação entre camadas
- **Exception**: Exceções customizadas para tratamento de erros

## 🎯 Boas Práticas Implementadas

### 🏛️ **Clean Architecture (Arquitetura Limpa)**

O projeto segue os princípios da Clean Architecture, garantindo:

- **Independência de frameworks**: O domínio não depende de tecnologias específicas
- **Testabilidade**: Facilita a criação de testes unitários
- **Independência de UI**: A lógica de negócio é independente da interface
- **Independência de banco de dados**: O domínio não conhece detalhes de persistência
- **Independência de agentes externos**: Regras de negócio isoladas de serviços externos

### 🔧 **SOLID Principles**

#### **S - Single Responsibility Principle (Princípio da Responsabilidade Única)**
```csharp
// Cada classe tem uma única responsabilidade
public class DoLoginUsuarioUseCase
{
    // Responsabilidade: Executar o caso de uso de login
}

public class RegisterUsuarioValidator : AbstractValidator<RequestRegisterUsuarioJson>
{
    // Responsabilidade: Validar dados de registro
}
```

#### **O - Open/Closed Principle (Princípio Aberto/Fechado)**
```csharp
// Aberto para extensão, fechado para modificação
public interface IUsuarioRepository
{
    Task<Usuario> GetByIdAsync(Guid id);
    Task<Usuario> GetByEmailAsync(string email);
    Task CreateAsync(Usuario usuario);
}
```

#### **L - Liskov Substitution Principle (Princípio da Substituição de Liskov)**
```csharp
// Implementações podem ser substituídas sem quebrar o código
public class UsuarioRepository : IUsuarioRepository
{
    // Implementação concreta que pode ser substituída
}
```

#### **I - Interface Segregation Principle (Princípio da Segregação de Interface)**
```csharp
// Interfaces específicas para cada responsabilidade
public interface ICryptographyService
{
    string HashPassword(string password);
    bool Verify(string password, Usuario usuario);
}

public interface ITokenService
{
    string GenerateToken(Usuario usuario);
}
```

#### **D - Dependency Inversion Principle (Princípio da Inversão de Dependência)**
```csharp
// Dependências são injetadas via construtor
public class RegisterUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly IValidator<RequestRegisterUsuarioJson> _registerValidator;
    private readonly ICryptographyService _algorithm;
    private readonly ITokenService _tokenService;
    
    public RegisterUsuarioUseCase(
        IUsuarioRepository repository,
        IValidator<RequestRegisterUsuarioJson> registerValidator,
        ICryptographyService algorithm,
        ITokenService tokenService)
    {
        // Injeção de dependências
    }
}
```

### 🏗️ **Domain-Driven Design (DDD)**

#### **Entidades de Domínio**
```csharp
public class Usuario
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public DateTime CreateAt { get; set; } = DateTime.Now;
}
```

#### **Interfaces de Domínio**
- Definição de contratos no domínio
- Implementações na infraestrutura
- Inversão de dependência

### 🔄 **Dependency Injection (Injeção de Dependência)**

```csharp
// Registro de serviços no Program.cs
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<DoLoginUsuarioUseCase>();
builder.Services.AddScoped<RegisterUsuarioUseCase>();
builder.Services.AddScoped<ICryptographyService, CryptographyService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();
```

### ✅ **Validation Pattern**

#### **FluentValidation**
```csharp
public class RegisterUsuarioValidator : AbstractValidator<RequestRegisterUsuarioJson>
{
    public RegisterUsuarioValidator() 
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MinimumLength(3).WithMessage("O nome deve conter no mínimo 3 caracteres.")
            .MaximumLength(100).WithMessage("O nome deve conter no máximo 100 caracteres.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("O e-mail é obrigátório.")
            .EmailAddress().WithMessage("O e-mail informado é inválido.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("A senha é obrigatória.")
            .MinimumLength(6).WithMessage("A senha deve conter mais do que 6 caracteres.");
    }
}
```

### 🚨 **Exception Handling Pattern**

#### **Exceções Customizadas**
```csharp
public class InvalidLoginException : LoginUsuarioException
{
    public InvalidLoginException() : base("Email ou senha inválidos.")
    {
    }
}

public class ErroNaValidacaoException : LoginUsuarioException
{
    public ErroNaValidacaoException(List<string> errors) : base(errors)
    {
    }
}
```

#### **Tratamento Centralizado**
```csharp
// Nos controllers
catch (LoginUsuarioException login)
{
    return Unauthorized(new ResponseErrorMessageJson
    {
        Errors = login.GetErrorMessage()
    });
}
```

### 📋 **Use Case Pattern**

#### **Caso de Uso de Login**
```csharp
public class DoLoginUsuarioUseCase
{
    private readonly IUsuarioRepository _repository;
    private readonly ICryptographyService _algorithm;
    private readonly ITokenService _tokenService;
    
    public async Task<ResponseLoginUsuarioJson> Execute(RequestLoginUsuarioJson request)
    {
        var usuario = await _repository.GetByEmailAsync(request.Email);

        if (usuario is null || !_algorithm.Verify(request.Password, usuario))
            throw new InvalidLoginException();

        return new ResponseLoginUsuarioJson
        {
            Name = usuario.Name,
            Token = _tokenService.GenerateToken(usuario)
        };
    }
}
```

### 🔐 **Security Patterns**

#### **Criptografia de Senhas**
- Uso de BCrypt para hash seguro
- Verificação de senhas sem armazenar em texto plano

#### **JWT Authentication**
- Tokens com tempo de expiração curto (1 minuto)
- Claims específicos para identificação do usuário
- Validação automática via middleware

### 📊 **Repository Pattern**

```csharp
public interface IUsuarioRepository
{
    Task<Usuario> GetByIdAsync(Guid id);
    Task<Usuario> GetByEmailAsync(string email);
    Task CreateAsync(Usuario usuario);
}
```

### 🎨 **DTO Pattern (Data Transfer Objects)**

#### **Separação de Request/Response**
```csharp
// Request DTOs
public class RequestLoginUsuarioJson
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

// Response DTOs
public class ResponseLoginUsuarioJson
{
    public string Name { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
```

### 🔧 **Configuration Pattern**

#### **Configuração Externa**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=KELVINKRAUSE;Database=UsuariosDb;..."
  },
  "JwtSettings": {
    "SecretKey": "M3Lk6L4j07aIxnMgoCJzXEzFZfoU00iZ"
  }
}
```

### 📝 **Naming Conventions**

- **PascalCase**: Classes, métodos, propriedades públicas
- **camelCase**: Parâmetros, variáveis locais
- **PREFIX**: Interfaces começam com "I" (IUsuarioRepository)
- **SUFFIX**: Use Cases terminam com "UseCase" (DoLoginUsuarioUseCase)

### 🧪 **Testability Patterns**

- **Injeção de Dependência**: Facilita mock de dependências
- **Interfaces**: Permite substituição por implementações de teste
- **Separação de Responsabilidades**: Cada classe pode ser testada isoladamente

## 🛠️ Tecnologias Utilizadas

### Backend
- **.NET 8** - Framework principal
- **Entity Framework Core 9.0.5** - ORM para acesso a dados
- **SQL Server** - Banco de dados
- **JWT Bearer** - Autenticação e autorização
- **FluentValidation 12.0.0** - Validação de dados
- **BCrypt** - Criptografia de senhas
- **Swashbuckle.AspNetCore 6.6.2** - Documentação da API

### Ferramentas de Desenvolvimento
- **Entity Framework Tools** - Migrations e gerenciamento do banco
- **Swagger/OpenAPI** - Documentação interativa da API

## 📋 Pré-requisitos

- .NET 8 SDK
- SQL Server (Local ou remoto)
- Visual Studio 2022 ou VS Code

## ⚙️ Configuração

### 1. Clone o repositório
```bash
git clone [URL_DO_REPOSITORIO]
cd LoginUsuario
```

### 2. Configure o banco de dados
Edite o arquivo `LoginUsuario.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=UsuariosDb;User Id=sa;Password=SUA_SENHA;TrustServerCertificate=True;"
  },
  "JwtSettings": {
    "SecretKey": "SUA_CHAVE_SECRETA_JWT"
  }
}
```

### 3. Execute as migrations
```bash
cd LoginUsuario.Api
dotnet ef database update
```

### 4. Execute a aplicação
```bash
dotnet run
```

A API estará disponível em: `https://localhost:7100`

## 📚 Documentação da API

### Endpoints Disponíveis

#### 🔐 Autenticação

##### POST `/api/Usuario/register`
Registra um novo usuário no sistema.

**Request Body:**
```json
{
  "name": "Kelvin Krause",
  "email": "kelvinkrause@gmail.com",
  "password": "senha123"
}
```

**Response (201 Created):**
```json
{
  "nome": "Kelvin Krause",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

##### POST `/api/Usuario/login`
Realiza o login do usuário e retorna um token JWT.

**Request Body:**
```json
{
  "email": "kelvinkrause@gmail.com",
  "password": "senha123"
}
```

**Response (200 OK):**
```json
{
  "nome": "Kelvin Krause",
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

#### 🔍 Validação de Sessão

##### GET `/api/Conectado/autenticado`
Verifica se o token está válido e retorna informações do usuário autenticado.

**Headers:**
```
Authorization: Bearer {seu_token_jwt}
```

**Response (200 OK):**
```json
{
  "mensagem": "Você esta autenticado.",
  "id": "guid-do-usuario",
  "email": "kelvinkrause@gmail.com",
  "nome": "Kelvin Krause"
}
```

### 🔒 Autenticação

Para acessar endpoints protegidos, inclua o header:
```
Authorization: Bearer {seu_token_jwt}
```

**Características do Token JWT:**
- Duração: 1 minuto
- Algoritmo: HMAC SHA256
- Claims incluídos: ID do usuário, email e nome

## 🧪 Testando a API

### Swagger UI
Acesse `https://localhost:7100/swagger` para documentação interativa da API.

### Exemplo com cURL

#### 1. Registrar usuário
```bash
curl -X POST "https://localhost:7100/api/Usuario/register" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Kelvin Krause",
    "email": "kelvinkrause@gmail.com",
    "password": "senha123"
  }'
```

#### 2. Fazer login
```bash
curl -X POST "https://localhost:7100/api/Usuario/login" \
  -H "Content-Type: application/json" \
  -d '{
    "email": "kelvinkrause@gmail.com",
    "password": "senha123"
  }'
```

#### 3. Validar token
```bash
curl -X GET "https://localhost:7100/api/Conectado/autenticado" \
  -H "Authorization: Bearer {token_obtido_no_login}"
```

## 🔧 Desenvolvimento

### Estrutura de Projeto

```
LoginUsuario/
├── 📁 LoginUsuario.Api/
│   ├── 📁 Controllers/             # Controladores da API
│   ├── 📄 Program.cs               # Configuração da aplicação
│   └── 📄 appsettings.json         # Configurações
├── 📁 LoginUsuario.Application/
│   ├── 📁 UseCases/                # Casos de uso
│   └── 📁 Validators/              # Validadores
├── 📁 LoginUsuario.Domain/
│   ├── 📁 Entities/                # Entidades do domínio
│   └── 📁 Interfaces/              # Contratos
├── 📁 LoginUsuario.Infrastructure/
│   ├── 📁 Data/                    # Contexto do banco
│   ├── 📁 Repositories/            # Implementação dos repositórios
│   └── 📁 Security/                # Serviços de segurança
├── 📁 LoginUsuario.Comunication/
│   ├── 📁 Requests/                # DTOs de entrada
│   └── 📁 Responses/               # DTOs de saída
└── 📁 LoginUsuario.Exception/      # Exceções customizadas
```

### Comandos Úteis

#### Criar nova migration
```bash
dotnet ef migrations add NomeDaMigration
```

#### Aplicar migrations
```bash
dotnet ef database update
```

#### Remover migration
```bash
dotnet ef migrations remove
```

## 🚨 Tratamento de Erros

A API retorna códigos de status HTTP apropriados:

- **200 OK**: Operação realizada com sucesso
- **201 Created**: Recurso criado com sucesso
- **400 Bad Request**: Dados inválidos ou erro de validação
- **401 Unauthorized**: Token inválido ou expirado
- **500 Internal Server Error**: Erro interno do servidor

### Exemplo de Resposta de Erro
```json
{
  "errors": [
    "Email já está em uso",
    "Senha deve ter pelo menos 6 caracteres"
  ]
}
```

## 🔐 Segurança

### Criptografia
- Senhas são criptografadas usando BCrypt
- Tokens JWT são assinados com chave secreta
- Validação de dados com FluentValidation

### Boas Práticas Implementadas
- Separação de responsabilidades (Clean Architecture)
- Injeção de dependência
- Tratamento de exceções centralizado
- Validação de entrada de dados
- Tokens JWT com tempo de expiração curto

## 🤝 Contribuição

1. Faça um fork do projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📞 Suporte

Para dúvidas, sugestões ou suporte técnico:

### 🐛 **Reportando Problemas**
- **Issues do GitHub**: [Criar uma sugestão](https://github.com/kelvinkrause/LoginUsuario/issues)
- **Email**: [kelvinrkrause@gmail.com]
- **LinkedIn**: [www.linkedin.com/in/kelvin-krause]

### 💡 **Sugestões e Melhorias**
- **Issues do GitHub**: [Criar uma sugestão](https://github.com/kelvinkrause/LoginUsuario/issues)
- **Email**: [kelvinrkrause@gmail.com]

---

**Desenvolvido com ❤️ para aprendizado e prática de desenvolvimento de APIs com .NET 8** 
