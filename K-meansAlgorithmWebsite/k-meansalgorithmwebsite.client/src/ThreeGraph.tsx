import React, { useRef, useEffect } from 'react';
import * as THREE from 'three';
import { OrbitControls } from 'three/examples/jsm/controls/OrbitControls';

type Props = {
  dataPoints: number[][];
  clusters: number[] | null | undefined;
};

function getRandomColor(): string {
  const letters = '0123456789ABCDEF';
  let color = '#';
  for (let i = 0; i < 6; i++) {
    color += letters[Math.floor(Math.random() * 16)];
  }
  return color;
}

const Graph3D: React.FC<Props> = ({ dataPoints, clusters }) => {
  const mountRef = useRef<HTMLDivElement>(null);
  const scene = new THREE.Scene();

  useEffect(() => {
    const camera = new THREE.PerspectiveCamera(75, window.innerWidth / window.innerHeight, 0.1, 10000);
    const renderer = new THREE.WebGLRenderer();
    renderer.setSize(900, 600);
    mountRef.current?.appendChild(renderer.domElement);

    const controls = new OrbitControls(camera, renderer.domElement);

    // Создание сетки
    const size = 50;
    const divisions = 50;
    const gridHelper = new THREE.GridHelper(size, divisions);
    gridHelper.position.x = 25;
    gridHelper.position.z = 25;
    scene.add(gridHelper);

    // Создание осей
    const axesHelper = new THREE.AxesHelper(50);
    scene.add(axesHelper);

    const colors: string[] = [];
    clusters?.forEach(() => {
      colors.push(getRandomColor());
    })

    
    const filteredDataPoints = dataPoints.filter(point => point.every(coord => coord >= 0 && coord <= 100));     
    filteredDataPoints.forEach((point, index) => {   
      const clusterIndex = clusters?.[index] ?? null;

      if (clusterIndex !== undefined && clusterIndex != null) {
        const [x, y, z] = point;
        const geometry = new THREE.SphereGeometry(0.5, 40, 40);
        const material = new THREE.MeshBasicMaterial({ color: colors[clusterIndex] });
        const sphere = new THREE.Mesh(geometry, material);
        sphere.position.set(x, y, z);
        scene.add(sphere);
        }
    });

    camera.position.x = 50;
    camera.position.y = 50;
    camera.position.z = 50;

    const animate = () => {
      requestAnimationFrame(animate);
      controls.update();
      renderer.render(scene, camera);
    };

    animate();

    return () => {
      mountRef.current?.removeChild(renderer.domElement);
    };
  }, [dataPoints]);

  return <div ref={mountRef} />;
};

export default Graph3D;
