using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private Transform _buildingDemolishButtonUI;
    [SerializeField] private Transform _buildingRepairButtonUI;
    
    private BuildingTypeHolder _buildingType;
    private HealthSystem _healthSystem;
    private ParticleSystem _buildingDestroyedParticles;
    private bool _canRepair;

    private void Start()
    {
        if (_buildingDemolishButtonUI != null)
        {
            _buildingDemolishButtonUI.gameObject.SetActive(false);
        }

        if (_buildingRepairButtonUI != null)
        {
            _buildingRepairButtonUI.gameObject.SetActive(false);
        }
        
        _buildingType = GetComponent<BuildingTypeHolder>();
        _healthSystem = GetComponent<HealthSystem>();
        _buildingDestroyedParticles = GetComponentInChildren<ParticleSystem>();

        _healthSystem.SetMaxHealthAmount(_buildingType.BuildingType.MaxHealthAmount, true);
        
        _healthSystem.OnDie += DestroyBuilding;
        _healthSystem.OnDamage += ToggleRepairButton;
        _healthSystem.OnHeal += ToggleRepairButton;
    }

    private void DestroyBuilding()
    {
        AudioManager.Instance.PlaySFX(AudioManager.SFX.BuildingDestroyed);
        CinemachineShake.Instance.ShakeCamera(9, 1);
        ChromaticAberrationEffect.Instance.SetChromaticAberrationWeight(0.8f);
        _buildingDestroyedParticles.Play();
        
        Destroy(gameObject, 0.5f);
    }

    private void OnMouseEnter()
    {
        ShowDemolishButton(true);
        ShowRepairButton(true);
    }

    private void OnMouseExit()
    {
        ShowDemolishButton(false);
        ShowRepairButton(false);
    }

    private void ShowDemolishButton(bool shouldShow)
    {
        if (_buildingDemolishButtonUI == null)
        {
            return;
        }
        
        _buildingDemolishButtonUI.gameObject.SetActive(shouldShow);
    }

    private void ShowRepairButton(bool shouldShow)
    {
        if (_buildingRepairButtonUI == null)
        {
            return;
        }

        if (shouldShow)
        {
            AudioManager.Instance.PlaySFX(AudioManager.SFX.BuildingDamaged);
            shouldShow = _canRepair;
        }
        
        _buildingRepairButtonUI.gameObject.SetActive(shouldShow);
    }
    
    private void ToggleRepairButton(bool isFullHealth)
    {
        _canRepair = !isFullHealth;
    }
}
