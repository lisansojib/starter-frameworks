# Infrastructure Documentation and Special instructions
#### Enabling Migration in a specific folder
``` 
1. enable-migrations -EnableAutomaticMigration:$false -MigrationsDirectory Migrations\CustomerDatabases Or
2. enable-migrations -EnableAutomaticMigration:$false -MigrationsDirectory Migrations\CustomerDatabases -ContextTypeName FullyQualifiedContextName
```
More Info - [stackoverflow discussion](https://stackoverflow.com/a/32227569)

#### Adding Migration
```
add-migrations "Migration Name"
add-migration "Migration Name" -IgnoreChanges
add-migration "Migration Name" -ConfigurationTypeName Infrastructure.Data.Migrations.Tex.Configuration -IgnoreChanges
```

#### Running Migration to DB
```
update-database -verbose
update-database -ConfigurationTypeName Infrastructure.Data.Migrations.Tex.Configuration -verbose
```