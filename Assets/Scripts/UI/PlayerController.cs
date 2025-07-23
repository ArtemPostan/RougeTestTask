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
        // 1) Преобразуем курсор в мировые координаты (2D)
        Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // 2) Ищем Collider2D под точкой
        Collider2D hit = Physics2D.OverlapPoint(wp);
        if (hit == null) return;

        // 3) Получаем компонент SelectableComponent
        var sel = hit.GetComponent<SelectableComponent>();
        if (sel == null) return;

        // 4) Снимаем подсветку с предыдущего, если была
        if (_selectedComp != null && _selectedComp != sel)
            _selectedComp.Highlight(false);

        // 5) Подсвечиваем новый, сохраняем и передаём в AttackComponent
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
