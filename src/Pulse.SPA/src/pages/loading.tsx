import React from "react";
import { Spinner } from "@heroui/react";

const LoadingPage: React.FC = () => {
  return (
    <div className="flex flex-col items-center justify-center min-h-[60vh]">
      <Spinner size="lg" />
      <p className="mt-4 text-default-600">Loading...</p>
    </div>
  );
};

export default LoadingPage;
