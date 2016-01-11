using Microsoft.Xna.Framework;

namespace TopDown
{
    public interface Entity
    {
        /// <summary>
        /// Get the position of the entity
        /// </summary>
        /// <returns></returns>
        Vector2 GetPosition();

        /// <summary>
        /// Get the rectangle of the entity
        /// </summary>
        /// <returns></returns>
        Rectangle GetRectangle();
    }
}
