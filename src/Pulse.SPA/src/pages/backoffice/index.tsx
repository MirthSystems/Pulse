import { title } from "@/components/primitives";

export default function BackofficePage() {
  return (
    <section className="flex flex-col items-center justify-center gap-4 py-8 md:py-10">
      <div className="inline-block max-w-lg text-center justify-center">
        <h1 className={title()}>Backoffice Dashboard</h1>
        <p className="text-default-600 mt-4">
          Welcome to the protected backoffice area. This is where administrative
          functions will be implemented.
        </p>
      </div>
    </section>
  );
}
