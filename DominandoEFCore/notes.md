# Dominando o Entity Framework Core

## EF Database

### Para que serve c Deleted/Created

- Alternativa às migrações, usados normalmente em ambiente de DEV.
- Ensure Created - cria o BD e tabelas (se não existir).
- Ensure Deleted - exclui um BD.

### Resolvendo GAP do EnsureCreated para múltiplos contextos