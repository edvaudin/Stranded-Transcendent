using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Ranger
{
    [SerializeField] GameObject healthBar;
    [SerializeField] RectTransform hudRect;
    [SerializeField] string bossName;
    List<GameObject> nodes;
    private int nodeSpawnInterval;
    private int nodeCount = 6;
    private GameObject node;
    protected override void Awake()
    {
        base.Awake();
        var healthBarInstance = Instantiate(healthBar, FindObjectOfType<HealthBarPanel>().transform).GetComponent<UISlider>();
        healthBarInstance.SetSliderText(bossName);
        health.SetSlider(healthBarInstance);
        node = new GameObject();
        GenerateNodes();
    }

    private void GenerateNodes()
    {
        nodes = new List<GameObject>();

        nodeSpawnInterval = 360 / nodeCount;
        float nodeSpawnAngle = 0;
        for (int i = 0; i < nodeCount; i++)
        {
            var nodeInstance = Instantiate(node, transform.position, Quaternion.Euler(new Vector3(0, 0, nodeSpawnAngle)), transform);
            nodeInstance.transform.Translate(Vector2.up * projectileSpawnGap);
            nodeSpawnAngle += nodeSpawnInterval;
            nodes.Add(nodeInstance);
        }
    }
    protected override void Fire()
    {
        var ran = Random.Range(0, 2);
        switch (ran)
        {
            case 0:
                FireAll();
                break;
            case 1:
                StartCoroutine(FireSpiral());
                break;
        }
        
    }

    private void FireAll()
    {
        foreach (var node in nodes)
        {
            var projectileInstance = Instantiate(projectile, node.transform.position, node.transform.rotation);
            projectileInstance.GetComponent<Projectile>().Launch(agent.velocity, projectileSpeed);
        }
        timeSinceFired = 0;
    } 

    private IEnumerator FireSpiral()
    {
        foreach (var node in nodes)
        {
            var projectileInstance = Instantiate(projectile, node.transform.position, node.transform.rotation);
            projectileInstance.GetComponent<Projectile>().Launch(agent.velocity, projectileSpeed);
            timeSinceFired = 0;
            yield return new WaitForSeconds(0.2f);
        }
        
    }
}
