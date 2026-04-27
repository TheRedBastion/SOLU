using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingEnemy : BaseEnemy
{
    [Header("Enemy Vars")]
    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float detectionRange = 14f;
    [SerializeField] private float orbitRange = 6f;
    [SerializeField] private float orbitDistance = 4f;
    [SerializeField] private float dashSpeed = 6f;
    [SerializeField] private float dashCooldown = 3.5f;

    private float dashTimer;
    private bool isDashing = false;

    private int orbitSide = 1;
    private int lockedSide = 1;


    private bool inReposition;
    private bool inAnglePhase;

    private Vector2 repositionTarget;
    private Vector2 angleTarget;
    private Vector2 arcForward;
    private bool arcLocked;
    private float wobbleTime;

    protected override void Update()
    {
        base.Update();

        if (knockback != null && (knockback.IsKnockedBack || knockback.IsStunned)) return;
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > detectionRange)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 toPlayer = player.position - transform.position;
        Vector2 dirToPlayer = toPlayer.normalized;
        Vector2 perp = new Vector2(-dirToPlayer.y, dirToPlayer.x);

        dashTimer -= Time.deltaTime;

        //---------------- DASH ----------------
        if (isDashing)
        {
            rb.linearVelocity = dirToPlayer * dashSpeed;

            if (distance < 0.5f)
            {
                isDashing = false;

                lockedSide = orbitSide;

                inReposition = true;
                inAnglePhase = false;

                //PURE LEFT/RIGHT SNAP
                repositionTarget = (Vector2)player.position + Vector2.right * lockedSide * orbitDistance;
            }

            UpdateFacingDirection(toPlayer.x);
            return;
        }

        //---------------- REPOSITION ----------------
        if (inReposition)
        {
            Vector2 move = repositionTarget - (Vector2)transform.position;

            rb.linearVelocity = move.normalized * speed;

            if (move.magnitude < 0.2f)
            {
                inReposition = false;
                inAnglePhase = true;
                wobbleTime = 0f;

                //ANGLED TARGET
                float angle = lockedSide == 1 ? 35f : 145f; // RIGHT HERE CHANGE THIS HERE MIGHYT BE SOMETHING
                Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;

                angleTarget = (Vector2)player.position + dir * orbitDistance;
            }

            UpdateFacingDirection(toPlayer.x);
            return;
        }

        //---------------- ANGLED HOVER ----------------
        if (inAnglePhase)
        {
            Vector2 move = angleTarget - (Vector2)transform.position;

            wobbleTime += Time.deltaTime;

            Vector2 baseMove = move.normalized;

            Vector2 wobble = perp * Mathf.Sin(wobbleTime * 2.5f) * 0.2f;

            Vector2 finalMove = (baseMove + wobble).normalized;

            rb.linearVelocity = finalMove * speed;

            if (move.magnitude < 0.15f)
            {
                inAnglePhase = false;

                Vector2 dir = Quaternion.Euler(0, 0, lockedSide == 1 ? 15f : 165f) * Vector2.right;

                arcForward = dir.normalized;
                arcLocked = true;
            }
            UpdateFacingDirection(toPlayer.x);
            return;
        }

        //---------------- NORMAL ORBIT ZONE ----------------
        if (distance <= orbitRange)
        {
            Vector2 toPlayerDir;
            if (dashTimer <= 0f)
            {
                isDashing = true;
                dashTimer = dashCooldown;

                toPlayerDir = (player.position - transform.position).normalized;
                Vector2 perpNow = new Vector2(-toPlayerDir.y, toPlayerDir.x);

                orbitSide = Vector2.Dot(perpNow, transform.position - player.position) > 0 ? 1 : -1;
                return;
            }

            toPlayerDir = (player.position - transform.position).normalized;

            // fallback if arc not locked yet
            if (!arcLocked)
                arcForward = Quaternion.Euler(0, 0, lockedSide == 1 ? 15f : 165f) * Vector2.right;

            perp = new Vector2(-arcForward.y, arcForward.x);

            float wobble = Mathf.Sin(Time.time * 2.5f) * 0.2f;

            Vector2 wiggledDir = (arcForward + perp * wobble).normalized;

            Vector2 orbitPos = (Vector2)player.position + wiggledDir * orbitDistance;

            Vector2 toOrbit = orbitPos - (Vector2)transform.position;

            Vector2 radialDir = (Vector2)player.position - (Vector2)transform.position;
            float distanceError = radialDir.magnitude - orbitDistance;

            Vector2 radialForce = radialDir.normalized * distanceError;

            Vector2 move =
                radialForce * 2.0f +
                wiggledDir * 1.2f;

            rb.linearVelocity = move.normalized * speed;

        }
        else
        {
            rb.linearVelocity = dirToPlayer * speed;
            UpdateFacingDirection(dirToPlayer.x);
        }
        UpdateFacingDirection(toPlayer.x);
    }

    public void SetDetectionRange(float newRange)
    {
        detectionRange = newRange;
    }

    void UpdateFacingDirection(float directionX)
    {
        Vector3 scale = transform.localScale;

        if (directionX > 0)
            scale.x = Mathf.Abs(scale.x);
        else if (directionX < 0)
            scale.x = -Mathf.Abs(scale.x);

        transform.localScale = scale;
    }
}
