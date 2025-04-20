# PROTOCOL: CharacterController Implementation

## IMPLEMENTATION STANDARDS

### Code Standards

- Follow Microsoft's C# Coding Conventions: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/coding-style/coding-conventions
- Maintain existing project naming conventions
- Add XML documentation for all public methods and properties
- Use Unity's recommended practices for CharacterController usage

### CharacterController Configuration

- Since this is a 2D game, configure the CharacterController with appropriate settings:
  - Set height and radius to match the character's 2D collider
  - Use lower skinWidth for 2D movement
  - Disable useGravity if not needed for the game
  - Set appropriate step offset (usually low or zero for 2D)
  - Set slope limit to maximum (90 degrees) for 2D movement

### Testing Procedure

1. Test basic movement in all directions
2. Test collision with environmental objects
3. Test movement at boundaries
4. Test integration with other game systems that depend on player position
5. Test performance under various conditions

## TRANSITION PLAN

### Phase 1: Dual-System Implementation

Initially, both systems (Rigidbody2D and CharacterController) may coexist to ensure a smooth transition:

- CharacterController will handle movement
- Rigidbody2D can remain for compatibility, but should be kinematic
- When fully tested, remove Rigidbody2D dependency

### Phase 2: Complete Transition

Once the CharacterController implementation is verified to work correctly:

- Remove Rigidbody2D component entirely
- Update all references to use CharacterController

## TECHNICAL CONSIDERATIONS

### 2D Adaptation

CharacterController is primarily designed for 3D, but can be adapted for 2D by:

- Using Move() with a 2D vector (with Z component set to 0)
- Constraining rotation to the Z-axis only
- Properly configuring the collider size to match the 2D sprite

### Performance Optimization

- Use Time.deltaTime for frame-rate independent movement
- Avoid calling CharacterController.Move() multiple times per frame
- Only compute collisions when necessary

## TROUBLESHOOTING

### Common Issues

1. **Problem:** Character doesn't move
   **Solution:** Check if the CharacterController is properly initialized and input is being received

2. **Problem:** Collisions not working properly
   **Solution:** Adjust the CharacterController's radius and height to match the character's visual representation

3. **Problem:** Character gets stuck on obstacles
   **Solution:** Adjust the step offset or implement custom logic to handle these cases

4. **Problem:** Movement feels different from Rigidbody2D
   **Solution:** Adjust movement parameters to match the feel of the original implementation

## VERIFICATION CHECKLIST

Before finalizing the implementation, verify the following:

- [ ] Character moves correctly in all directions
- [ ] Character collides properly with environment
- [ ] Movement boundaries are respected
- [ ] Performance is acceptable under all conditions
- [ ] All existing gameplay mechanics that depend on player movement continue to work
