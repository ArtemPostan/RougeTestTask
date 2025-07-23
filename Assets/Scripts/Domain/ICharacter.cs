/// <summary>
/// Интерфейс для любого «боевого» персонажа: у него есть здоровье, атака и баффы.
/// Позволяет обращаться к игроку и врагам единым способом.
/// </summary>
public interface ICharacter
{
    HealthComponent Health { get; }
    AttackComponent Attack { get; }
    BuffComponent Buffs { get; }
}
