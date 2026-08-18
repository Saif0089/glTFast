// SPDX-FileCopyrightText: 2024 Unity Technologies and the glTFast authors
// SPDX-License-Identifier: Apache-2.0

using GLTFast.Schema;
using NUnit.Framework;
using UnityEngine;

namespace GLTFast.Tests
{
    class VertexBufferDescriptorTests
    {

        [Test]
        public void VertexBufferDescriptorEqualTest()
        {
            var a = VertexBufferDescriptor.FromPrimitive(
                new MeshPrimitive { attributes = new Attributes { POSITION = 42 } });
            var b = VertexBufferDescriptor.FromPrimitive(
                new MeshPrimitive { attributes = new Attributes { POSITION = 42 } });

            Assert.IsTrue(a == b);

            a = VertexBufferDescriptor.FromPrimitive(
                new MeshPrimitive { attributes = new Attributes { POSITION = 41 } });
            b = VertexBufferDescriptor.FromPrimitive(
                new MeshPrimitive { attributes = new Attributes { POSITION = 42 } });

            Assert.IsTrue(a == b);
        }

#if DRACO_IS_INSTALLED
        [Test]
        public void DracoCompressedPrimitiveIsNotEqualToUncompressed()
        {
            var uncompressed = VertexBufferDescriptor.FromPrimitive(
                new MeshPrimitive { attributes = new Attributes { POSITION = 0 } });
            var draco = VertexBufferDescriptor.FromPrimitive(
                new MeshPrimitive
                {
                    attributes = new Attributes { POSITION = 0 },
                    extensions = new MeshPrimitiveExtensions
                    {
                        KHR_draco_mesh_compression = new MeshPrimitiveDracoExtension
                        {
                            bufferView = 0,
                            attributes = new Attributes { POSITION = 0 }
                        }
                    }
                });

            // Identical attributes, so everything else about them matches. They must
            // still not share a PrimitiveSet: the set's mesh generator is chosen off
            // its first primitive, and DracoMeshGenerator asserts on an uncompressed
            // one (and dereferences the missing extension in a build without
            // assertions).
            Assert.AreNotEqual(uncompressed, draco);
            Assert.IsTrue(uncompressed != draco);
            Assert.AreNotEqual(uncompressed.GetHashCode(), draco.GetHashCode());
        }
#endif
    }
}
