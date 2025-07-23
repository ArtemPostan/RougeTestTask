/// <summary>
/// ��������� ��� ������ �������� ���������: � ���� ���� ��������, ����� � �����.
/// ��������� ���������� � ������ � ������ ������ ��������.
/// </summary>
public interface ICharacter
{
    HealthComponent Health { get; }
    AttackComponent Attack { get; }
    BuffComponent Buffs { get; }
}
