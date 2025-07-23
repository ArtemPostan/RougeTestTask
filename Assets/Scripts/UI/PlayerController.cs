using UnityEngine;

[RequireComponent(typeof(AttackComponent), typeof(ManaComponent))]
public class PlayerController : MonoBehaviour
{
    private AttackComponent _attack;
    private AbilityComponent[] _abilities;
    private ICharacter _selectedChar;
    private SelectableComponent _selectedComp;

    public void Initialize()
    {
        _attack = GetComponent<AttackComponent>();
        _abilities = GetComponents<AbilityComponent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            HandleSelection();
    }

    private void HandleSelection()
    {
        // 1) ����������� ������ � ������� ���������� (2D)
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2) ���� Collider2D ��� ������
        Collider2D hit = Physics2D.OverlapPoint(wp);
        if (hit == null) return;

        // 3) �������� ��������� SelectableComponent
        var sel = hit.GetComponent<SelectableComponent>();
        if (sel == null) return;

        // 4) ������� ��������� � �����������, ���� ����
        if (_selectedComp != null && _selectedComp != sel)
            _selectedComp.Highlight(false);

        // 5) ������������ �����, ��������� � ������� � AttackComponent
        sel.Highlight(true);
        _selectedComp = sel;
        _selectedChar = sel.GetComponent<ICharacter>();
        _attack.SetTarget(_selectedChar);

        _selectedChar.Health.OnDeath += ClearSelection;
    }

    public void UseAbility(int index)
    {
        if (_selectedChar == null) return;
        if (index < 0 || index >= _abilities.Length) return;

        _abilities[index].Use(_selectedChar);
    }

    public void ClearSelection()
    {
        if (_selectedComp != null)
        {
            _selectedComp.Highlight(false);
            _selectedComp = null;
        }
        _selectedChar = null;
        _attack.SetTarget(null);
    }
}
