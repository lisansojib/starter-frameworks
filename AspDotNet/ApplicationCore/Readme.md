# Application Core Developer Guides

### Entity Class Rules
1. Primary Key field name must be **Id** to support **Generic Repository Pattern**.
2. Entity class requires a parameterless constructor. It can be public, internal or private.
3. **Optional:** If you want to use Select dropdown with your class using generic repository pattern,
Name your Name Property as **Name**