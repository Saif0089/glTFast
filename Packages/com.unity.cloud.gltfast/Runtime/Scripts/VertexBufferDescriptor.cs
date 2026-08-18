// SPDX-FileCopyrightText: 2024 Unity Technologies and the glTFast authors
// SPDX-License-Identifier: Apache-2.0

#nullable enable
using System;
using GLTFast.Schema;

namespace GLTFast
{
    readonly struct VertexBufferDescriptor : IEquatable<VertexBufferDescriptor>
    {
        readonly bool m_HasNormals;
        readonly bool m_HasTangents;

        readonly int m_TexCoordCount;
        readonly bool m_HasColors;

        readonly bool m_HasBones;
        readonly int m_MorphTargetCount;

        /// <summary>
        /// Draco compressed primitives are decoded by <see cref="DracoMeshGenerator"/> and
        /// uncompressed ones by <see cref="MeshGenerator"/>, but the generator is picked once
        /// per <see cref="PrimitiveSet"/>, off its first primitive. Without this, a compressed
        /// and an uncompressed primitive whose attributes match would land in the same set and
        /// whichever came second would be handed to the wrong generator.
        /// </summary>
        readonly bool m_IsDracoCompressed;

        VertexBufferDescriptor(
            bool hasNormals,
            bool hasTangents,
            int texCoordCount,
            bool hasColors,
            bool hasBones,
            int morphTargetCount,
            bool isDracoCompressed
            )
        {
            m_HasNormals = hasNormals;
            m_HasTangents = hasTangents;
            m_TexCoordCount = texCoordCount;
            m_HasColors = hasColors;
            m_HasBones = hasBones;
            m_MorphTargetCount = morphTargetCount;
            m_IsDracoCompressed = isDracoCompressed;
        }

        static bool IsDracoCompressed(MeshPrimitiveBase primitive)
        {
#if DRACO_IS_INSTALLED
            return primitive.IsDracoCompressed;
#else
            return false;
#endif
        }

        public static VertexBufferDescriptor FromPrimitive(MeshPrimitiveBase primitive)
        {
            return new VertexBufferDescriptor(
                primitive.attributes.NORMAL >= 0,
                primitive.attributes.TANGENT >= 0,
                primitive.attributes.GetTexCoordsCount(),
                primitive.attributes.COLOR_0 >= 0,
                primitive.attributes.WEIGHTS_0 >= 0 && primitive.attributes.JOINTS_0 >= 0,
                primitive.targets?.Length ?? 0,
                IsDracoCompressed(primitive)
            );
        }

        public override int GetHashCode()
        {
#if NET_STANDARD
            return HashCode.Combine(
                m_HasNormals,
                m_HasTangents,
                m_TexCoordCount,
                m_HasColors,
                m_HasBones,
                m_MorphTargetCount,
                m_IsDracoCompressed
            );
#else
            var hash = 13;
            if (m_HasNormals)
                hash = hash * 31 + 13;
            if (m_HasTangents)
                hash = hash * 31 + 14;
            hash = hash * 31 + m_TexCoordCount;
            if (m_HasColors)
                hash = hash * 31 + 15;
            if (m_HasBones)
                hash = hash * 31 + 16;
            hash = hash * 31 + m_MorphTargetCount;
            if (m_IsDracoCompressed)
                hash = hash * 31 + 17;
            return hash;
#endif
        }

        public override bool Equals(object? obj) => obj is VertexBufferDescriptor other && Equals(other);

        public bool Equals(VertexBufferDescriptor other)
        {
            return m_HasNormals == other.m_HasNormals
                && m_HasTangents == other.m_HasTangents
                && m_TexCoordCount == other.m_TexCoordCount
                && m_HasColors == other.m_HasColors
                && m_HasBones == other.m_HasBones
                && m_MorphTargetCount == other.m_MorphTargetCount
                && m_IsDracoCompressed == other.m_IsDracoCompressed;
        }

        public static bool operator ==(VertexBufferDescriptor lhs, VertexBufferDescriptor rhs) => lhs.Equals(rhs);

        public static bool operator !=(VertexBufferDescriptor lhs, VertexBufferDescriptor rhs) => !(lhs == rhs);
    }
}
