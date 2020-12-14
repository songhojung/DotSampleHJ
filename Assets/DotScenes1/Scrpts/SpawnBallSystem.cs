using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;

public class SpawnBallSystem : SystemBase
{
    BeginInitializationEntityCommandBufferSystem EntityCommandBufferSystem;
    private float time = 0f;

    protected override void OnCreate()
    {
        EntityCommandBufferSystem = World.GetOrCreateSystem<BeginInitializationEntityCommandBufferSystem>();

    }

    protected override void OnUpdate()
    {
        //var commandbuffer = EntityCommandBufferSystem.CreateCommandBuffer().Asparellerwriter



        time += Time.DeltaTime;

        if (time > 1f)
        {
            time = 0f;

            var commandBuffer = EntityCommandBufferSystem.CreateCommandBuffer();

            var enManager = EntityManager;
            Random ran = new Random(50);

            Entities
            .ForEach((Entity entity, in ShootPointComponent shootPtData, in Translation location, in LocalToWorld SpawnTranform) =>
            {

                {
                    Entity ballEtt = commandBuffer.Instantiate(shootPtData.BallPrefab);

                    float3 pos = location.Value;

                    PhysicsVelocity velocity = new PhysicsVelocity()
                    {
                        Linear = SpawnTranform.Forward + new float3(0.5f,0f,0.3f) * 20f, 
                        Angular = float3.zero,
                    };


                    commandBuffer.SetComponent(ballEtt, new Translation { Value = pos });
                    commandBuffer.SetComponent(ballEtt, velocity);




                }

            }).Run();


            EntityCommandBufferSystem.AddJobHandleForProducer(Dependency);
        }
    }
}
