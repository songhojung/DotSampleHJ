using System;
using System.Runtime.InteropServices.ComTypes;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Extensions;
using Unity.Transforms;
using UnityEngine;

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



        time += Time.DeltaTime;

        if (/*time > 1f*/Input.GetKeyDown(KeyCode.Space))
        {
            time = 0f;

            var commandBuffer = EntityCommandBufferSystem.CreateCommandBuffer();

            var enManager = EntityManager;
          


            Entities
            .ForEach((Entity entity, in ShootPointComponent shootPtData, in Translation location, in LocalToWorld SpawnTranform) =>
            {
                Unity.Mathematics.Random ran = new Unity.Mathematics.Random((uint)UnityEngine.Random.Range(1, 100000));
                float3 ranVec = new float3(ran.NextFloat(-1f, 1f), 0, ran.NextFloat(0.3f, 1f));

                {
                    Entity ballEtt = commandBuffer.Instantiate(shootPtData.BallPrefab);

                    float3 pos = location.Value;

                    //float3 ranVec = ran.NextFloat3Direction();

                    Debug.Log(string.Format("Ran : {0}",ranVec));

                    PhysicsVelocity velocity = new PhysicsVelocity()
                    {
                        Linear = SpawnTranform.Forward + ranVec * 20f, 
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
