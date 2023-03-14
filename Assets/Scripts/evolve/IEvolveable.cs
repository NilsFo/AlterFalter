using UnityEngine;

public interface IEvolveable
{
    
    [ContextMenu("Evolve Now!")]
    public void Evolve();

    public bool CanEvolve();

    public void OnEvolve();
}