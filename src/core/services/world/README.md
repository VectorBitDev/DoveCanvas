# DoveCanvas ECS

DoveCanvas uses a lightweight Entity Component System (ECS).

- **Entities** are just IDs.
- **Components** are plain data.
- **Systems** contain logic.
- **Queries** efficiently retrieve entities matching a set of components.

---

# Creating Entities

```csharp
Entity player = Services.World.CreateEntity();

Entity enemy = Services.World.CreateEntity();
```

Destroying an entity:

```csharp
Services.World.DestroyEntity(player);
```

---

# Components

Create a component by inheriting from `Component`.

```csharp
public sealed class Transform : Component
{
    public Vector2 Position;
    public float Rotation;
}
```

Another component:

```csharp
public sealed class Velocity : Component
{
    public Vector2 Value;
}
```

---

# Adding Components

```csharp
Entity player = Services.World.CreateEntity();

Services.World.AddComponent(player, new Transform()
{
    Position = new Vector2(100, 50),
    Rotation = 0
});

Services.World.AddComponent(player, new Velocity()
{
    Value = new Vector2(1, 0)
});
```

---

# Removing Components

```csharp
Services.World.RemoveComponent<Velocity>(player);
```

---

# Checking Components

```csharp
if (Services.World.HasComponent<Transform>(player))
{
    Console.WriteLine("Player has a Transform.");
}
```

---

# Getting Components

```csharp
Transform transform = Services.World.GetComponent<Transform>(player);

transform.Position.X += 10;
```

---

# TryGetComponent

```csharp
if (Services.World.TryGetComponent(player, out Velocity? velocity))
{
    Console.WriteLine(velocity.Value);
}
```

---

# Creating Queries

Query all entities with a Transform.

```csharp
WorldQuery query = Services.World
    .Query()
    .With<Transform>();
```

---

Query entities with multiple components.

```csharp
WorldQuery query = Services.World
    .Query()
    .With<Transform>()
    .With<Velocity>();
```

---

Exclude components.

```csharp
WorldQuery query = Services.World
    .Query()
    .With<Transform>()
    .Without<Disabled>();
```

---

# Fetching Entities

```csharp
IReadOnlyList<Entity> entities = Services.World
    .Query()
    .With<Transform>()
    .Fetch();

foreach (Entity entity in entities)
{
    Console.WriteLine(entity.Id);
}
```

---

# Enumerating Queries

`WorldQuery` implements `IEnumerable<Entity>`.

```csharp
WorldQuery query = Services.World
    .Query()
    .With<Transform>();

foreach (Entity entity in query)
{
    Transform transform = Services.World.GetComponent<Transform>(entity);

    Console.WriteLine(transform.Position);
}
```

---

# Common Workflow

```csharp
Entity player = Services.World.CreateEntity();

Services.World.AddComponent(player, new Transform());
Services.World.AddComponent(player, new Velocity());
Services.World.AddComponent(player, new Sprite());

foreach (Entity entity in Services.World
    .Query()
    .With<Transform>()
    .With<Sprite>())
{
    Transform transform = Services.World.GetComponent<Transform>(entity);
    Sprite sprite = Services.World.GetComponent<Sprite>(entity);

    // Render entity
}
```

---

# Best Practices

- Components should only contain data.
- Put game logic inside systems.
- Reuse `WorldQuery` instances instead of creating new ones every frame.
- Prefer `TryGetComponent()` when a component is optional.
- Destroy entities instead of manually removing every component.
- Keep components small and focused.

---

# Example

```csharp
Entity player = Services.World.CreateEntity();

Services.World.AddComponent(player, new Transform()
{
    Position = new Vector2(200, 150)
});

Services.World.AddComponent(player, new Velocity()
{
    Value = new Vector2(50, 0)
});

WorldQuery movers = Services.World
    .Query()
    .With<Transform>()
    .With<Velocity>();

foreach (Entity entity in movers)
{
    Transform transform = Services.World.GetComponent<Transform>(entity);
    Velocity velocity = Services.World.GetComponent<Velocity>(entity);

    transform.Position += velocity.Value * Time.DeltaTime;
}
```
