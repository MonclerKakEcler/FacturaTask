namespace Factura.Enemy
{
    public class EnemyModel
    {
        public float Health { get; private set; } = 100f;
        public float Speed { get; private set; } = 5f;
        public float DetectionRadius { get; private set; } = 100f;
    }
}
