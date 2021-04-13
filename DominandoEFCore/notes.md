# Dominando o Entity Framework Core

## EF Database

### Para que serve c Deleted/Created

- Alternativa às migrações, usados normalmente em ambiente de DEV.
- Ensure Created - cria o BD e tabelas (se não existir).
- Ensure Deleted - exclui um BD.

### Resolvendo GAP do EnsureCreated para múltiplos contextos

- Para exemplificar foi criado uma classe Cidade e um contexto novo para essa classe, porém usando a mesma string de conexão;
- Na classe Program foi criado um método novo - GapDoEnsureCreated();
- Nesse método estamos tentando usar o EnsureCreated para cada contexto;
- Ao rodar a aplicação, só é criado as tabelas do primeiro contexto - esse é o GAP;
- Não é possível rodar o EnsureCreated para contextos diferentes que usam o mesmo BD;
- Resolvendo - Criamos o databaseCreator e usamos o método CreateTables();

### HealthCheck do banco de dados

- Validando a conexão;
- Vamos criar o método na classe Program - HealthCheckBancoDeDados();