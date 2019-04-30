using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SmoothDamp<T> {

    protected T m_target;
    protected T m_current;
    protected T m_smooth;
    protected float m_damping;

    public T value
    {
        get { return m_current; }
        set { m_current = value; m_target = value; }
    }

    public T target
    {
        get { return m_target; }
        set { m_target = value; }
    }
    
    public float damping
    {
        get { return m_damping; }
        set { m_damping = value; }
    }

    public SmoothDamp(float _damping = 0.0f)
    {
        m_damping = _damping;
    }

    public abstract void Update();
}

public class SmoothVector2 : SmoothDamp<Vector2>
{
    public SmoothVector2(Vector2 _startValue, float _damping = 0.0f)
    {
        m_damping = _damping;
        m_target = _startValue;
        m_current = _startValue;
    }

    public override void Update()
    {
        m_current = Vector2.SmoothDamp(m_current, m_target, ref m_smooth, m_damping);
    }
}


public class SmoothVector3 : SmoothDamp<Vector3>
{
    public SmoothVector3(Vector3 _startValue, float _damping = 0.0f)
    {
        m_damping = _damping;
        m_target = _startValue;
        m_current = _startValue;
    }

    public override void Update()
    {
        m_current = Vector3.SmoothDamp(m_current, m_target, ref m_smooth, m_damping);
    }
}


public class SmoothFloat : SmoothDamp<float>
{
    public SmoothFloat(float _startValue, float _damping = 0.0f)
    {
        m_damping = _damping;
        m_target = _startValue;
        m_current = _startValue;
    }

    public override void Update()
    {
        m_current = Mathf.SmoothDamp(m_current, m_target, ref m_smooth, m_damping);
    }
}