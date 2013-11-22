using JamGame.GameObjects;
using Microsoft.Xna.Framework;

namespace JamGame.Components
{
    public class MotionEngine
    {
        #region Vars

        public Vector2 GoalVelocity = Vector2.Zero;

        private readonly GameObject character;

        private Vector2 oldPosition = Vector2.Zero;

        private Vector2 oldVelocity = Vector2.Zero;

        private Vector2 floor = Vector2.Zero;

        private float factor = 50;

        #endregion

        #region Properties

        /// <summary>
        /// Palauttaa edellisen paikan ennen nopeuden laskua tällä framella.
        /// Hyödyllinen esim collisionissa
        /// </summary>
        public Vector2 OldPosition
        {
            get { return oldPosition; }
        }

        /// <summary>
        /// Palauttaa viime framen nopeuden.
        /// </summary>
        public Vector2 OldVelocity
        {
            get { return oldVelocity; }
        }

        public Vector2 Direction
        {
            get;
            set;
        }

        public Vector2 OldDirection
        {
            get;
            protected set;
        }

        public float GoalVelocityX
        {
            get { return GoalVelocity.X; }
            set { GoalVelocity.X = value; }
        }

        public float GoalVelocityY
        {
            get { return GoalVelocity.Y; }
            set { GoalVelocity.Y = value; }
        }
        public bool Enabled
        {
            get;
            set;
        }
        #endregion

        public MotionEngine(GameObject target)
        {
            character = target;
            OldDirection = Vector2.Zero;

            Enabled = true;
        }

        #region Methods

        public void Update(GameTime gameTime)
        {
            if (Enabled)
            {
                Vector2 v = new Vector2();

                v.X = CalculateSpeed(GoalVelocity.X, character.Velocity.X, (float)gameTime.ElapsedGameTime.TotalSeconds * factor);
                v.Y = CalculateSpeed(GoalVelocity.Y, character.Velocity.Y, (float)gameTime.ElapsedGameTime.TotalSeconds * factor);

                oldVelocity = character.Velocity;
                character.Velocity = v;

                oldPosition = character.Position;
                character.Position += character.Velocity;
            }
        }

        public float CalculateSpeed(float goal, float currentSpeed, float deltaTime)
        {
            float diff = goal - currentSpeed;
            // jos ollaan kiihtymässä
            if (diff > deltaTime)
            {
                return currentSpeed + deltaTime;
            }

            // jos ollaan pysähtymässä
            if (diff < -deltaTime)
            {
                return currentSpeed - deltaTime;
            }

            return goal;
        }

        #endregion
    }
}
